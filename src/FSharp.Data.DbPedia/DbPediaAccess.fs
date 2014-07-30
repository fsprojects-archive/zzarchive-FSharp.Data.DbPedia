module DbPediaAccess

open RestSharp
open FSharp.Data
open FSharp.Data.JsonExtensions
open System.Text.RegularExpressions
open System.Globalization
open System.Linq
open System.Linq.Expressions

let typeOrArrayElementType (ty : System.Type) =
        if ty.IsArray then
            ty.GetElementType()
        else
            ty

let extractObj (pattern : string) input =
    let escapedPattern = pattern |> String.collect (fun c -> if "[](){}.*?^$".Contains(c.ToString()) then "\\" + c.ToString() else c.ToString())
    let regexPattern = "^" + escapedPattern.Replace("%s", "(.+?)").Replace("%d", "(-?\d+)").Replace("%f", "(-?\d+\.?\d*)") + "$"
    let m = Regex.Match(input, regexPattern, RegexOptions.Compiled)
    let types = pattern.Split('%') 
                |> Array.map (fun s -> s.[0]) 
                |> Array.filter (fun c -> "sdf".Contains(c.ToString()))
                |> Array.map (fun c -> match c with 
                                        | 's' -> typeof<string> 
                                        | 'd' -> typeof<int> 
                                        | 'f' -> typeof<float> 
                                        | _ -> failwith "Unexpected datatype pattern")
    let groups = [| for g in m.Groups -> g |].[1..]     // collect all matching groups except the first (which matches the entire input)
    let parsedValues = groups 
                        |> Array.map2 (fun ty gp -> match ty with
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
     day : int}

type DbPediaConnection(locale: string, limit: int) as this =

    // the REST client
    let client = new RestClient("http://dbpedia.org/sparql")
    do client.AddHandler("application/sparql-results+json", new Deserializers.JsonDeserializer())

    let jsonToFsharpValue (record : JsonValue) =
        let value : string = record?value.AsString()
        match record?``type``.AsString() with
        | "typed-literal" -> 
            match record?datatype.AsString() with
            | "http://www.w3.org/2001/XMLSchema#integer" -> box (System.Int32.Parse(value))
            | "http://www.w3.org/2001/XMLSchema#float" -> box (System.Single.Parse(value))
            | "http://www.w3.org/2001/XMLSchema#double" -> box (System.Double.Parse(value))
            | "http://www.w3.org/2001/XMLSchema#gYear" 
            | "http://www.w3.org/2001/XMLSchema#date" -> box (System.DateTime.Parse(value))
            | "http://dbpedia.org/datatype/second" -> box (System.Single.Parse(value))
            | "http://dbpedia.org/datatype/poundSterling"
            | "http://dbpedia.org/datatype/usDollar" -> box (System.Int64.Parse(value, NumberStyles.AllowDecimalPoint ||| NumberStyles.AllowExponent))
            | "http://dbpedia.org/datatype/squareKilometre" -> box (System.Single.Parse(value))
            | "http://dbpedia.org/datatype/inhabitantsPerSquareKilometre" -> box (System.Double.Parse(value))
            | "http://dbpedia.org/datatype/inch" -> box (System.Single.Parse(value))
            | "http://www.openlinksw.com/schemas/virtrdf#Geometry" -> 
                let long, lat = extract<float*float> "POINT(%f %f)" value
                box {latitude=lat; longitude=long}
            | "http://www.w3.org/2001/XMLSchema#gMonthDay" -> 
                let m, d = extract<int*int> "--%d-%d" value
                box {month=m; day=d}
            | _ -> failwithf "Unrecognized datatype: %s\nOffending JSON context:\n%A" (record?datatype.AsString()) record
        | "uri" -> 
            if value.StartsWith("http://dbpedia.org/resource/") then
                let label = System.Uri.UnescapeDataString(value.Replace("http://dbpedia.org/resource/", ""))
                box (DbPediaIndividualBase(this, value, label))
            else    
                box value
        | "literal" -> box value
        | _ -> failwithf "Unrecognized property type: %s" (record?``type``.AsString())

    // takes a list of tuples (p,_,t,_) of property-type pairs,
    // and for any contiguous duplicate properties:
    // 1) merges them into a single element; and
    // 2) changes the type to an array type
    // e.g. [(p,l,int,v); (p,l,int,v)] becomes [(p,l,int[],v)]
    let rec mergeProperties list =
        match list with
        | [] -> []
        | [x] -> [x]
        | (k1,l1,t1,v1)::(k2,l2,t2:System.Type,v2)::tail -> 
            if k1 = k2 && l1 = l2 then
                mergeProperties ((k1, l1, t2.MakeArrayType(), v1)::tail)
            else
                (k1, l1, t1, v1)::(mergeProperties ((k2, l2, t2, v2)::tail))

//    let rec dbpRefListToStringList list =
//        match list with
//        | [] -> []
//        | [x] -> [x]
//        | (k1,l1,t1,v1)::(k2,l2,t2,v2)::tail -> 
//            if k1 = k2 && t1 = typeof<DbPediaIndividualBase> && t2 = typeof<string> then
                

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
                               let pValue = jsonToFsharpValue r?value
                               (pUri, pLabel, pValue.GetType(), pValue))
        |> Array.sortBy (fun (uri,_,ty,_) -> (uri, ty.Name))        // ensure all the dbpedia refs are contiguous
        |> Array.toList
//        |> dbpRefListToStringList
        |> List.map (fun (u,label,ty,v) -> if ty = typeof<DbPediaIndividualBase> then 
                                                (u, label+" (dbpedia ref)", ty, v)
                                           else
                                                (u, label, ty, v))
        |> mergeProperties

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
        |> Array.map (fun r -> unbox<'T>(jsonToFsharpValue r?value))

    member __.getPropertyValue<'T> entityUri propertyUri =
        (__.getPropertyValues<'T> entityUri propertyUri).[0]

    member __.tryGetPropertyValue<'T> entityUri propertyUri =
        match (__.getPropertyValues<'T> entityUri propertyUri) with
        | [| |] -> None
        | array -> Some array.[0]

    member __.getIndividuals uri =
        let query = sprintf "SELECT * WHERE {?uri rdf:type <%s>} ORDER BY ?uri LIMIT %d" uri limit

        let data = __.ExecuteRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> 
                            let uriString = r?uri?value.AsString()
                            let label = System.Uri.UnescapeDataString(uriString.Replace("http://dbpedia.org/resource/", "")).Replace("_", " ")
                            uriString, label)

and DbPediaIndividualBase(conn: DbPediaConnection, uri, label) =
    member __.Connection = conn
    member __.Uri = uri
    member __.Name = label
    member __.Abstract = conn.tryGetPropertyValue<string> uri "http://dbpedia.org/ontology/abstract"
    member __.Thumbnail = conn.tryGetPropertyValue<string> uri "http://dbpedia.org/ontology/thumbnail"
    member __.GetProperties() = (conn.getPropertyBag uri) |> List.map (fun (u,l,t,v) -> u,l,t)
    member __.GetPropertyValue(propertyUri) = conn.getPropertyValues uri propertyUri

type DbPediaDataContextBase(locale: string, limit: int) =
    let conn = DbPediaConnection(locale, limit)
    member __.Connection = conn

type DbPediaIndividualsTypeBase(conn: DbPediaConnection) =
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

    member __.Connection = conn