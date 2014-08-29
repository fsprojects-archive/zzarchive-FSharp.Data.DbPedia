module DbPediaAccess

open RestSharp
open FSharp.Data
open FSharp.Data.JsonExtensions
open System.Text.RegularExpressions
open System.Linq
open System.Linq.Expressions
open DbPediaMeasures.MeasureHelpers

/// Conditionally folds two contiguous items (.[i] and .[i+1]) in a list by replacing them with Some(result) 
/// or leaves both items alone if 'folder' returns None for that pair. The sliding window then advances to .[i+1] and .[i+2]
/// and the process repeats.
let rec foldWhen folder list =
    match list with
    | [] -> []
    | [x] -> [x]
    | a::b::tail ->
        match folder(a,b) with
        | Some x -> foldWhen folder (x::tail)
        | None -> a::(foldWhen folder (b::tail))

let typeOrArrayElementType (ty : System.Type) =
        if ty.IsArray then
            ty.GetElementType()
        else
            ty

let extractObj (pattern : string) input =
    let escapedPattern = pattern |> String.collect (fun c -> if "[](){}.*?^$".Contains(c.ToString()) then "\\" + c.ToString() else c.ToString())
    let regexPattern = "^" + escapedPattern.Replace("%s", "(.+?)").Replace("%d", "(-?\d+)").Replace("%f", "(-?\d+\.?\d*)") + "$"
    let m = Regex.Match(input, regexPattern, RegexOptions.Compiled)
    let types = 
        pattern.Split('%') 
        |> Array.map (fun s -> s.[0]) 
        |> Array.filter (fun c -> "sdf".Contains(c.ToString()))
        |> Array.map (fun c -> 
            match c with 
            | 's' -> typeof<string> 
            | 'd' -> typeof<int> 
            | 'f' -> typeof<float> 
            | _ -> failwith "Unexpected datatype pattern")
    let groups = [| for g in m.Groups -> g |].[1..]     // collect all matching groups except the first (which matches the entire input)
    let parsedValues = 
        groups 
        |> Array.map2 (fun ty gp -> 
            match ty with
            | t when t = typeof<string> -> gp.Value :> obj
            | t when t = typeof<int> -> System.Int32.Parse(gp.Value) :> obj
            | t when t = typeof<float> -> System.Single.Parse(gp.Value) :> obj 
            | _ -> failwith "Unexpected datatype") types
    let tupleType = Microsoft.FSharp.Reflection.FSharpType.MakeTupleType types
    Microsoft.FSharp.Reflection.FSharpValue.MakeTuple(parsedValues, tupleType)

let extract<'T> pattern input = (extractObj pattern input) :?> 'T

let failOnce f =
    let savedError = ref None
    (fun x ->
      match savedError.Value with
      | Some exn -> raise exn
      | None ->
        try f x 
        with exn ->
           savedError := Some exn
           raise exn)

let failAlways f x = f x

type Geography = 
    {latitude : float
     longitude : float}

type MonthDay =
    {month : int
     day : int }

type YearMonth =
    {year : int
     month : int }

type Time =
    {hours : int
     minutes : int
     seconds : float
     timeZoneUtcOffset : System.TimeSpan option }

type DbPediaConnection(locale: string, limit: int) as this =

    // the REST client
    let client = new RestClient("http://dbpedia.org/sparql")
    do client.AddHandler("application/sparql-results+json", new Deserializers.JsonDeserializer())

    let jsonToFsharpValue (record : JsonValue) =
        let value : string = record?value.AsString()
        match record?``type``.AsString() with
        | "typed-literal" -> 
            match record?datatype.AsString() with
            | "http://www.w3.org/2001/XMLSchema#float" -> float value |> box, typeof<float>
            | "http://www.w3.org/2001/XMLSchema#double" -> double value |> box, typeof<double>
            | "http://www.w3.org/2001/XMLSchema#boolean" -> System.Boolean.Parse(value) |> box, typeof<bool>
            | "http://www.w3.org/2001/XMLSchema#integer"
            | "http://www.w3.org/2001/XMLSchema#positiveInteger"
            | "http://www.w3.org/2001/XMLSchema#nonPositiveInteger"
            | "http://www.w3.org/2001/XMLSchema#negativeInteger"
            | "http://www.w3.org/2001/XMLSchema#nonNegativeInteger" -> int value |> box, typeof<int>
            | "http://www.w3.org/2001/XMLSchema#gYear" -> int value |> box, typeof<int>
            | "http://www.w3.org/2001/XMLSchema#date" -> System.DateTime.Parse(value) |> box, typeof<System.DateTime>
            | "http://www.w3.org/2001/XMLSchema#gDay" -> extract<int> "---%d" value |> box, typeof<int>
            | "http://www.w3.org/2001/XMLSchema#gMonth" -> extract<int> "--%d" value |> box, typeof<int>
            | "http://www.w3.org/2001/XMLSchema#gMonthDay" -> 
                let mm, dd = extract<int*int> "--%d-%d" value
                box {month=mm; day=dd}, typeof<MonthDay>
            | "http://www.w3.org/2001/XMLSchema#gYearMonth" -> 
                let yyyy, mm = extract<int*int> "%d-%d" value
                box {year=yyyy; month=mm}, typeof<YearMonth>
            | "http://www.w3.org/2001/XMLSchema#time" -> 
                let m = Regex.Match(value, @"(\d\d):(\d\d):(\d\d)(\.\d+)?(Z|([+-]\d\d):(\d\d))?")
                let hrs = m.Groups.[1].Value |> int
                let mins = m.Groups.[2].Value |> int
                let secs = m.Groups.[3].Value + m.Groups.[4].Value |> float
                let zone = 
                    match m.Groups.[5].Value with
                    | "" -> None
                    | "Z" -> Some System.TimeSpan.Zero
                    | _ -> Some (System.TimeSpan(int(m.Groups.[6].Value), int(m.Groups.[7].Value), 0))
                box {hours = hrs; minutes = mins; seconds = secs; timeZoneUtcOffset = zone}, typeof<Time>
            | "http://www.openlinksw.com/schemas/virtrdf#Geometry" -> 
                let long, lat = extract<float*float> "POINT(%f %f)" value
                box {latitude=lat; longitude=long}, typeof<Geography>
            | DataType(f, rty) -> f value, rty
        | "uri" -> 
            if value.StartsWith("http://dbpedia.org/resource/") then
                let label = System.Uri.UnescapeDataString(value.Replace("http://dbpedia.org/resource/", ""))
                box (DbPediaIndividualBase(this, value, label)), typeof<DbPediaIndividualBase>
            else    
                box value, typeof<string>
        | "literal" -> box value, typeof<string>
        | _ -> failwithf "Unrecognized property type: %s" (record?``type``.AsString())

//    // takes a list of tuples (p,_,t,_) of property-type pairs,
//    // and for any contiguous duplicate properties:
//    // 1) merges them into a single element; and
//    // 2) changes the type to an array type
//    // e.g. [(p,l,int,v); (p,l,int,v)] becomes [(p,l,int[],v)]
//    let rec mergeProperties list =
//        match list with
//        | [] -> []
//        | [x] -> [x]
//        | (k1,l1,t1,v1)::(k2,l2,t2:System.Type,v2)::tail -> 
//            if k1 = k2 && l1 = l2 then
//                mergeProperties ((k1, l1, t2.MakeArrayType(), v1)::tail)
//            else
//                (k1, l1, t1, v1)::(mergeProperties ((k2, l2, t2, v2)::tail))
//
//    let rec preferPropertiesWithMeasures list =
//        match list with
//        | [] -> []
//        | [x] -> [x]
//        | (k1,l1,t1:System.Type,v1)::(k2,l2,t2:System.Type,v2)::tail -> 
//            if l1 = l2 then
//                if t1.IsNested && not t2.IsNested then
//                    preferPropertiesWithMeasures ((k1,l1,t1,v1)::tail)
//                elif not t1.IsNested && t2.IsNested then
//                    preferPropertiesWithMeasures ((k2,l2,t2,v2)::tail)
//                else
//                    (k1, l1, t1, v1)::(preferPropertiesWithMeasures ((k2,l2,t2,v2)::tail))
//            else
//                (k1, l1, t1, v1)::(preferPropertiesWithMeasures ((k2,l2,t2,v2)::tail))

    member __.ExecuteRequest = 
      failAlways
      //failOnce 
         (fun (query : string) ->
            let request = new RestRequest(Method.GET)
            request.AddHeader("Accept", "text/html, application/xhtml+xml, */*") |> ignore
            request.AddParameter("default-graph-uri", "http://dbpedia.org") |> ignore
            request.AddParameter("query", query) |> ignore
            request.AddParameter("format", "application/sparql-results+json") |> ignore
            request.AddParameter("timeout", "3000") |> ignore
            request.AddParameter("debug", "on") |> ignore 
            JsonValue.Parse(client.Execute(request).Content ))

    member __.getCategorySubclasses uri =
        let query =
            "SELECT * 
            WHERE { 
                ?uri skos:broader <" + uri + ">
            } 
            ORDER BY ?uri"

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> r?uri?value.AsString(), 
                               r?uri?value.AsString().Replace("http://dbpedia.org/resource/Category:", "")
                                                     .Replace("_", " "))

    member __.getOntologySubclasses uri =
        let query =
            "SELECT * 
            WHERE { 
                ?uri rdfs:subClassOf <" + uri + ">.
            } 
            ORDER BY ?uri"

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> r?uri?value.AsString(), 
                               r?uri?value.AsString().Replace("http://dbpedia.org/ontology/", ""))

    member __.searchOntology ontologyName (searchTerm : string) =
        let query = sprintf """SELECT * WHERE {?uri rdf:type dbpedia-owl:%s. FILTER(regex(?uri, "%s")) }""" ontologyName (searchTerm.Replace(" ", "_"))

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> 
                        let uri = r?uri?value.AsString()
                        let label = System.Uri.UnescapeDataString(uri.Replace("http://dbpedia.org/resource/", "")).Replace("_", " ")
                        uri, label)

    member __.getPropertyBag uri =
        let query =
            "SELECT * 
            WHERE { 
                <" + uri + "> ?property ?value
            }"

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.filter (fun r -> match r?value.TryGetProperty("xml:lang") with
                                    | None -> true
                                    | Some lang -> lang.AsString() = locale)
        |> Array.map (fun r -> let pUri = r?property?value.AsString()
                               let pLabel = pUri.[pUri.LastIndexOfAny([|'/';'#'|])+1 ..]
                               let pValue, pType = jsonToFsharpValue r?value
                               (pUri, pLabel, pType, pValue))
        |> Array.sortBy (fun (uri,_,ty,_) -> uri)        // ensure all the common properties are contiguous
        |> Array.toList
//        |> dbpRefListToStringList
        |> List.map (fun (u,label,ty,v) -> 
            match v with
            | :? DbPediaIndividualBase -> u, label+" (dbpedia ref)", ty, v
            | _ -> u, label, ty, v )
        |> foldWhen (fun (e1,e2) ->     //mergeProperties
            let k1,l1,t1,v1 = e1
            let k2,l2,t2,v2 = e2
            if k1 = k2 && l1 = l2 then
                Some (k1, l1, t2.MakeArrayType(), v1)
            else
                None ) 
        |> List.sortBy (fun (_,label,_,_) -> label)
        |> foldWhen (fun (e1,e2) ->     // preferPropertiesWithMeasures
            let _,l1,t1,_ = e1
            let _,l2,t2,_ = e2
            if l1 = l2 then
                if t1.IsNested && not t2.IsNested then
                    Some e1
                elif not t1.IsNested && t2.IsNested then
                    Some e2
                else
                    None
            else
                None )

    member __.getPropertyValues<'T> entityUri propertyUri =
        let query =
            "SELECT * 
            WHERE { 
                <" + entityUri + "> <" + propertyUri + "> ?value
            }"

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.filter (fun r -> match r?value.TryGetProperty("xml:lang") with
                                    | None -> true
                                    | Some lang -> lang.AsString() = locale)
        |> Array.map (fun r -> 
            let result = jsonToFsharpValue r?value
            unbox<'T>(fst result))

    member __.getPropertyValue<'T> entityUri propertyUri =
        (__.getPropertyValues<'T> entityUri propertyUri).[0]

    member __.tryGetPropertyValue<'T> entityUri propertyUri =
        match (__.getPropertyValues<'T> entityUri propertyUri) with
        | [| |] -> None
        | array -> Some array.[0]

    member __.getIndividuals (uri : string) =
        let predicate = if uri.StartsWith("http://dbpedia.org/ontology/") then "rdf:type" else "dcterms:subject"
        let query = sprintf "SELECT ?uri WHERE {?uri %s <%s>} ORDER BY ?uri LIMIT %d" predicate uri limit

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> 
                            let uriString = r?uri?value.AsString()
                            let label = System.Uri.UnescapeDataString(uriString.Replace("http://dbpedia.org/resource/", "")).Replace("_", " ")
                            uriString, label)
    
    member __.getIndividualsByLetter (uri: string) (letter: char) =
        let predicate = if uri.StartsWith("http://dbpedia.org/ontology/") then "rdf:type" else "dcterms:subject"
        let query = sprintf """SELECT ?uri, ?label WHERE 
                               {?uri %s <%s>; 
                                     rdfs:label ?label 
                                FILTER regex(?label, "^%c") 
                               } ORDER BY ?uri LIMIT %d""" predicate uri letter limit

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> 
                            let uriString = r?uri?value.AsString()
                            let label = r?label?value.AsString()
                            uriString, label)

and IDbPediaObject =
    abstract member Connection : DbPediaConnection

and DbPediaIndividualBase(conn: DbPediaConnection, uri, label) =
    member __.Uri = uri
    member __.Name = label
    member __.Abstract = conn.tryGetPropertyValue<string> uri "http://dbpedia.org/ontology/abstract"
    member __.Thumbnail = conn.tryGetPropertyValue<string> uri "http://dbpedia.org/ontology/thumbnail"
    member __.GetProperties() = (conn.getPropertyBag uri) |> List.map (fun (u,l,t,v) -> u,l,t)
    member __.GetPropertyValue(propertyUri) = conn.getPropertyValues uri propertyUri

    interface IDbPediaObject with
        member __.Connection = conn

type DbPediaDataContextBase(locale: string, limit: int) =
    let conn = DbPediaConnection(locale, limit)
    interface IDbPediaObject with
        member __.Connection = conn

type DbPediaIndividualsTypeBase(conn: DbPediaConnection) =
    interface IDbPediaObject with
        member __.Connection = conn

type DbPediaOntologyTypeBase(conn: DbPediaConnection, uri: string) as this =
    let individuals = conn.getIndividuals uri
    let data = seq {for uri, label in individuals do
                        yield new DbPediaIndividualBase(conn, uri, label)}

    interface System.Collections.Generic.IEnumerable<DbPediaIndividualBase> with
        member __.GetEnumerator() = data.GetEnumerator()

    interface System.Collections.IEnumerable with
        member __.GetEnumerator() = data.GetEnumerator() :> System.Collections.IEnumerator

    interface System.Linq.IQueryable<DbPediaIndividualBase> with
        member x.Provider = { new IQueryProvider with
            member x.CreateQuery(e: Expression): IQueryable = failwith "not used?"
            
            member x.CreateQuery<'T>(e: Expression): IQueryable<'T> = failwith "not implemented"
//                let optSelect = tryFindInTree (function Select _ -> true | _ -> false) e
//            
//                let optIterVar = 
//                    match optSelect with
//                    | Some (:? MethodCallExpression as selectNode) -> tryFindInTree (function Var _ -> true | _ -> false) selectNode.Arguments.[1] 
//                    | None -> failwith "Missing select clause"
//                    | Some _ -> failwith "optSelect is not a MethodCallExpression"
//
//                let iterVar = 
//                    match optIterVar with
//                    | Some (:? ParameterExpression as varNode) -> varNode.Name
//                    | None -> failwith "Missing iteration variable"
//                    | Some other -> failwithf "optIterVar is not a ParameterExpression, it is a %A" other.NodeType
//            
//                let sparqlQuery = {SelectFields = ["?" + iterVar]
//                                   WhereClause = [{Subject="?"+iterVar; Predicate="rdf:type"; Object="<"+uri+">"}]  }
//                printfn "SPARQL query:\n%O" sparqlQuery
//                
//                let data = conn.ExecuteRequest (sparqlQuery.ToString())
//                let results = 
//                    data?results?bindings.AsArray()
//                    |> Array.map (fun r -> new DbPediaIndividualBase(conn, r.GetProperty(iterVar)?value.AsString(), "test"))
//                
//                results.AsQueryable<'T>()
            
            member x.Execute(expression: Expression): obj = failwith "not used?"
            
            member x.Execute<'T>(e: Expression): 'T = failwith "not used?"
                
        }
        member x.Expression = Expression.Constant(x, typeof<IQueryable<DbPediaIndividualBase>>) :> Expression
        member x.ElementType = typeof<DbPediaIndividualBase>

    interface IDbPediaObject with
        member __.Connection = conn

type DbPediaCategoryTypeBase(conn: DbPediaConnection, uri: string) as this =
    let individuals = conn.getIndividuals uri
    let data = seq {for uri, label in individuals do
                        yield new DbPediaIndividualBase(conn, uri, label)}

    interface System.Collections.Generic.IEnumerable<DbPediaIndividualBase> with
        member __.GetEnumerator() = data.GetEnumerator()

    interface System.Collections.IEnumerable with
        member __.GetEnumerator() = data.GetEnumerator() :> System.Collections.IEnumerator

    interface System.Linq.IQueryable<DbPediaIndividualBase> with
        member x.Provider = { new IQueryProvider with
            member x.CreateQuery(e: Expression): IQueryable = failwith "not used?"
            
            member x.CreateQuery<'T>(e: Expression): IQueryable<'T> = failwith "not implemented"
//                let optSelect = tryFindInTree (function Select _ -> true | _ -> false) e
//            
//                let optIterVar = 
//                    match optSelect with
//                    | Some (:? MethodCallExpression as selectNode) -> tryFindInTree (function Var _ -> true | _ -> false) selectNode.Arguments.[1] 
//                    | None -> failwith "Missing select clause"
//                    | Some _ -> failwith "optSelect is not a MethodCallExpression"
//
//                let iterVar = 
//                    match optIterVar with
//                    | Some (:? ParameterExpression as varNode) -> varNode.Name
//                    | None -> failwith "Missing iteration variable"
//                    | Some other -> failwithf "optIterVar is not a ParameterExpression, it is a %A" other.NodeType
//            
//                let sparqlQuery = {SelectFields = ["?" + iterVar]
//                                   WhereClause = [{Subject="?"+iterVar; Predicate="rdf:type"; Object="<"+uri+">"}]  }
//                printfn "SPARQL query:\n%O" sparqlQuery
//                
//                let data = conn.ExecuteRequest (sparqlQuery.ToString())
//                let results = 
//                    data?results?bindings.AsArray()
//                    |> Array.map (fun r -> new DbPediaIndividualBase(conn, r.GetProperty(iterVar)?value.AsString(), "test"))
//                
//                results.AsQueryable<'T>()
            
            member x.Execute(expression: Expression): obj = failwith "not used?"
            
            member x.Execute<'T>(e: Expression): 'T = failwith "not used?"
                
        }
        member x.Expression = Expression.Constant(x, typeof<IQueryable<DbPediaIndividualBase>>) :> Expression
        member x.ElementType = typeof<DbPediaIndividualBase>

    interface IDbPediaObject with
        member __.Connection = conn