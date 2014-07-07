module DbPediaAccess

open RestSharp
open FSharp.Data
open FSharp.Data.JsonExtensions

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

type DbPediaConnection(locale: string, limit: int) =

    // the REST client
    let client = new RestClient("http://dbpedia.org/sparql")
    do client.AddHandler("application/sparql-results+json", new Deserializers.JsonDeserializer())

    let executeRequest = 
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

    let jsonToFsharpValue (record : JsonValue) =
        match record?``type``.AsString() with
        | "typed-literal" -> match record?datatype.AsString() with
                             | "http://www.w3.org/2001/XMLSchema#integer" -> box(System.Int32.Parse(record?value.AsString()))
                             | "http://dbpedia.org/datatype/second" -> box(System.Single.Parse(record?value.AsString()))
                             | _ -> failwith ("Unrecognized datatype: " + record?datatype.AsString())
        | "uri" -> box(record?value.AsString())
        | "literal" -> box(record?value.AsString())
        | _ -> failwith ("Unrecognized property type: " + record?``type``.AsString())

    // takes a list of tuples (p,_,t) of property-type pairs,
    // and for any contiguous duplicate properties:
    // 1) merges them into a single element; and
    // 2) changes the type to an array type
    // e.g. [(p,l,int); (p,l,int)] becomes [(p,l,int[])]
    let rec mergeProperties list =
        match list with
        | [] -> []
        | [x] -> [x]
        | (k1,l1,t1)::(k2,l2,t2:System.Type)::tail -> 
            if k1 = k2 then
                mergeProperties ((k1, l1, t2.MakeArrayType())::tail)
            else
                (k1, l1, t1)::(mergeProperties ((k2, l2, t2)::tail))


    member __.getOntologySubclasses uri =
        let query =
            "SELECT * 
            WHERE { 
                ?uri rdfs:subClassOf <" + uri + ">
            } 
            ORDER BY ?uri"

        let data = executeRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> r?uri?value.AsString(), 
                               r?uri?value.AsString().Replace("http://dbpedia.org/ontology/", "") )

    member __.getPropertyBag uri =
        let query =
            "SELECT * 
            WHERE { 
                <" + uri + "> ?property ?value
            }"

        let data = executeRequest query
        data?results?bindings.AsArray()
        |> Array.filter (fun r -> match r?value.TryGetProperty("xml:lang") with
                                    | None -> true
                                    | Some lang -> lang.AsString() = locale)
        |> Array.map (fun r -> let pUri = r?property?value.AsString()
                               let pLabel = pUri.[pUri.LastIndexOfAny([|'/';'#'|])+1 ..]
                               let pType = (jsonToFsharpValue r?value).GetType()
                               (pUri, pLabel, pType))
        |> Array.toList
        |> mergeProperties

    member __.getPropertyValues<'T> entityUri propertyUri =
        let query =
            "SELECT * 
            WHERE { 
                <" + entityUri + "> <" + propertyUri + "> ?value
            }"

        let data = executeRequest query
        data?results?bindings.AsArray()
        |> Array.filter (fun r -> match r?value.TryGetProperty("xml:lang") with
                                    | None -> true
                                    | Some lang -> lang.AsString() = locale)
        |> Array.map (fun r -> unbox<'T>(jsonToFsharpValue r?value))

    member conn.getPropertyValue<'T> entityUri propertyUri =
        (conn.getPropertyValues<'T> entityUri propertyUri).[0]

    member __.tryGetPropertyValue<'T> entityUri propertyUri =
        match (__.getPropertyValues<'T> entityUri propertyUri) with
        | [| |] -> None
        | array -> Some array.[0]

    member __.getIndividuals uri =
        let query = sprintf "SELECT * WHERE {?uri rdf:type <%s>} LIMIT %d" uri limit

        let data = executeRequest query
        data?results?bindings.AsArray()
        |> Array.map (fun r -> r?uri?value.AsString(),                                              // uri string
                               r?uri?value.AsString().Replace("http://dbpedia.org/resource/", "")   // display label
                                                     .Replace("_", " "))

