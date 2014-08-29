// Copyright (c) Microsoft Corporation 2005-2011.
// This sample code is provided "as is" without warranty of any kind. 
// We disclaim all warranties, either express or implied, including the 
// warranties of merchantability and fitness for a particular purpose. 

namespace FSharp.Data.DbPediaTypeProvider

open System
open System.Reflection
open System.Collections.Generic
open System.Linq
open System.Linq.Expressions
open Samples.FSharp.ProvidedTypes
open FSharp.Data
open FSharp.Data.JsonExtensions
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open DbPediaAccess

[<AutoOpen>]
module Helpers = 

    let fake<'T> = Unchecked.defaultof<'T>

    let makeHelper q ty = 
        let minfo = 
            match q with
            | Application(Application(Let(_,_,Lambda(_,Lambda(_,Call(_,minfo,_)))),_),_) -> minfo
            | _ -> failwith "expected a different quotation pattern!"
        let gminfo = minfo.GetGenericMethodDefinition()
        gminfo.MakeGenericMethod( [|  ty |])

    let makeGetPropertyValuesHelper ty = makeHelper <@ (fake<DbPediaConnection>).getPropertyValues<int> fake fake @> ty
    let makeGetPropertyValueHelper ty = makeHelper  <@ (fake<DbPediaConnection>).getPropertyValue<int> fake fake @> ty

type DbPediaTypeFactory(schemaConn : DbPediaConnection) =
    let serviceTypes = ProvidedTypeDefinition("ServiceTypes", Some typeof<obj>, HideObjectMethods=true)
    let typeCache = Dictionary<string, ProvidedTypeDefinition>()

    member __.GetServiceTypes() = serviceTypes
     
    member __.MakePropertyForPropertyOfIndividual(entityUri, propUri, propName, propType:Type, propValue) =
        let compileTimeType =
            match unbox<obj>(propValue) with
            | :? DbPediaIndividualBase as entity -> 
                if propType.IsArray then
                    typeof<DbPediaIndividualBase>.MakeArrayType()
                else
                    __.MakeTypeForIndividual(entity.Uri) :> System.Type
            | _ -> propType
        
        let runtimeType = if propType.IsNested && propType.IsGenericType then propType.DeclaringType else propType    // strip any units of measure info for runtime type

        ProvidedProperty(propName, compileTimeType, 
            GetterCode=  
                (fun args -> 
                        let connExpr = <@@ ((%%(args.[0]) : DbPediaIndividualBase) :> IDbPediaObject).Connection @@>
                        let minfo = 
                            if runtimeType.IsArray then
                                let ty = runtimeType.GetElementType() 
                                makeGetPropertyValuesHelper ty
                            else
                                makeGetPropertyValueHelper runtimeType
                        Expr.Call(connExpr, minfo, [ <@@ entityUri @@>; <@@ propUri @@> ])))
                            
    member __.MakeTypeForIndividual(uri : string) =
        if typeCache.ContainsKey(uri) then
            typeCache.Item(uri)
        else
            let label = System.Uri.UnescapeDataString(uri.Replace("http://dbpedia.org/resource/", "")) + "Type"
            let t = ProvidedTypeDefinition(label, Some typeof<DbPediaIndividualBase>, HideObjectMethods=true)
            t.AddMembersDelayed(fun () -> 
                uri
                |> schemaConn.getPropertyBag
                |> List.map (fun (pUri, pLabel, pType, pValue) -> __.MakePropertyForPropertyOfIndividual(uri, pUri, pLabel, pType, pValue)))
            serviceTypes.AddMember(t)
            typeCache.Add(uri, t)
            t
        
    member __.MakePropertyForIndividual(uri, label) =
        let p = ProvidedProperty(label, __.MakeTypeForIndividual(uri), GetterCode= (fun args -> <@@ new DbPediaIndividualBase( ((%%(args.[0]) : DbPediaIndividualsTypeBase) :> IDbPediaObject).Connection, uri, label ) @@>  ))
        p.AddXmlDocDelayed(fun () -> 
            match schemaConn.tryGetPropertyValue uri "http://www.w3.org/2000/01/rdf-schema#comment" with
            | Some comment -> comment
            | None -> "No description available." )
        p

// This defines the type provider. When compiled to a DLL it can be added as a reference to an F#
// command-line compilation, script or project.
[<TypeProvider>]
type DbPediaTypeProvider(config: TypeProviderConfig) as this = 

    // Inheriting from this type provides implementations of ITypeProvider in terms of the
    // provided types below.
    inherit TypeProviderForNamespaces()
    
    let namespaceName = "FSharp.Data"
    let thisAssembly = Assembly.GetExecutingAssembly()

    let createTypesForStaticParameters(typeName, locale, limit, sample) = 
        let schemaConn = DbPediaConnection(locale, limit)
        let typeFactory = DbPediaTypeFactory(schemaConn)
        let instantiatedTy = ProvidedTypeDefinition(thisAssembly, namespaceName, typeName, Some typeof<obj>)

        let makeTypeForIndividualsX (uri: string) (letter: char) =
            let label = uri.Replace("http://dbpedia.org/ontology/", "").Replace("http://dbpedia.org/resource/Category:", "")
            let t = ProvidedTypeDefinition(label + "Individuals" + letter.ToString() + "Type", 
                                           Some typeof<DbPediaIndividualsTypeBase>, HideObjectMethods=true)
            t.AddMembersDelayed(fun () -> 
                schemaConn.getIndividualsByLetter uri letter
                |> Array.map (fun e -> typeFactory.MakePropertyForIndividual e) 
                |> Array.toList )
            typeFactory.GetServiceTypes().AddMember(t)
            t

        let makePropertyXForIndividualsAZType (uri: string) (letter: char) =
            ProvidedProperty(letter.ToString(), makeTypeForIndividualsX uri letter, 
                             GetterCode= (fun args -> <@@ new DbPediaIndividualsTypeBase( ((%%(args.[0]) : DbPediaIndividualsTypeBase) :> IDbPediaObject).Connection) @@> ))

        let makeTypeForIndividualsAZ (uri : string) =
            let label = uri.Replace("http://dbpedia.org/ontology/", "").Replace("http://dbpedia.org/resource/Category:", "")
            let t = ProvidedTypeDefinition(label + "IndividualsAZType", Some typeof<DbPediaIndividualsTypeBase>, HideObjectMethods=true)
            t.AddMembersDelayed(fun () -> 
                ['A'..'Z'] 
                |> List.map (makePropertyXForIndividualsAZType uri) )
            typeFactory.GetServiceTypes().AddMember(t)
            t

        let makeTypeForIndividuals (uri : string) =
            let label = uri.Replace("http://dbpedia.org/ontology/", "").Replace("http://dbpedia.org/resource/Category:", "")
            let t = ProvidedTypeDefinition(label + "IndividualsType", Some typeof<DbPediaIndividualsTypeBase>, HideObjectMethods=true)
            t.AddMembersDelayed(fun () -> 
                uri
                |> schemaConn.getIndividuals
                |> Array.map (fun e -> typeFactory.MakePropertyForIndividual e) 
                |> Array.toList )
            typeFactory.GetServiceTypes().AddMember(t)
            t

        let rec makeTypeForOntologyType (uri : string) =
            let label = uri.Replace("http://dbpedia.org/ontology/", "")
            let t = ProvidedTypeDefinition(label + "OntologyType", Some typeof<DbPediaOntologyTypeBase>, HideObjectMethods=true)
            t.AddMemberDelayed(fun () -> ProvidedProperty("Individuals", makeTypeForIndividuals uri, GetterCode= (fun args -> <@@ new DbPediaIndividualsTypeBase( ((%%(args.[0]) : DbPediaOntologyTypeBase) :> IDbPediaObject).Connection ) @@> )))
            t.AddMemberDelayed(fun () -> ProvidedProperty("IndividualsAZ", makeTypeForIndividualsAZ uri, GetterCode= (fun args -> <@@ new DbPediaIndividualsTypeBase( ((%%(args.[0]) : DbPediaOntologyTypeBase) :> IDbPediaObject).Connection ) @@> )))
            t.AddMembersDelayed(fun () -> uri
                                            |> schemaConn.getOntologySubclasses
                                            |> Array.map (fun topic -> makePropertyForOntologyType topic) 
                                            |> Array.toList )
            typeFactory.GetServiceTypes().AddMember(t)
            t

        and makePropertyForOntologyType (uri, label) =
            ProvidedProperty(label, makeTypeForOntologyType uri, GetterCode= (fun args -> <@@ new DbPediaOntologyTypeBase( ((%%(args.[0]) : DbPediaOntologyTypeBase) :> IDbPediaObject).Connection, uri ) @@> ))

        let ontologyType =
            let t = ProvidedTypeDefinition("OntologyType", Some typeof<DbPediaOntologyTypeBase>, HideObjectMethods=true)
            t.AddMembersDelayed(fun () -> "http://www.w3.org/2002/07/owl#Thing"
                                            |> schemaConn.getOntologySubclasses
                                            |> Array.map (fun topic -> makePropertyForOntologyType topic)
                                            |> Array.toList)
            typeFactory.GetServiceTypes().AddMember(t)
            t

        let rec makeTypeForCategoryType (uri : string) =
            let label = uri.Replace("http://dbpedia.org/resource/Category", "")
            let t = ProvidedTypeDefinition(label + "CategoryType", Some typeof<DbPediaCategoryTypeBase>, HideObjectMethods=true)
            t.AddMemberDelayed(fun () -> ProvidedProperty("Individuals", makeTypeForIndividuals uri, GetterCode= (fun args -> <@@ new DbPediaIndividualsTypeBase( ((%%(args.[0]) : DbPediaCategoryTypeBase) :> IDbPediaObject).Connection ) @@> )))
            t.AddMemberDelayed(fun () -> ProvidedProperty("IndividualsAZ", makeTypeForIndividualsAZ uri, GetterCode= (fun args -> <@@ new DbPediaIndividualsTypeBase( ((%%(args.[0]) : DbPediaCategoryTypeBase) :> IDbPediaObject).Connection ) @@> )))
            t.AddMembersDelayed(fun () -> 
                uri
                |> schemaConn.getCategorySubclasses
                |> Array.map (fun topic -> makePropertyForCategoryType topic) 
                |> Array.toList )
            typeFactory.GetServiceTypes().AddMember(t)
            t

        and makePropertyForCategoryType (uri, label) =
            ProvidedProperty(label, makeTypeForCategoryType uri, GetterCode= (fun args -> <@@ new DbPediaCategoryTypeBase( ((%%(args.[0]) : DbPediaCategoryTypeBase) :> IDbPediaObject).Connection, uri ) @@> ))
        
        let categoriesType =
            let t = ProvidedTypeDefinition("MainTopicsType", Some typeof<DbPediaCategoryTypeBase>, HideObjectMethods=true)
            t.AddMembersDelayed(fun () -> 
                "http://dbpedia.org/resource/Category:Main_topic_classifications"
                |> schemaConn.getCategorySubclasses
                |> Array.map (fun topic -> makePropertyForCategoryType topic)
                |> Array.toList)
            typeFactory.GetServiceTypes().AddMember(t)
            t

        let dbPediaDataContextType =
            let t = ProvidedTypeDefinition("DbPediaDataContext", Some typeof<DbPediaDataContextBase>, HideObjectMethods=true)
            let ontologyProperty = 
                ProvidedProperty("Ontology", ontologyType, 
                    GetterCode= (fun args -> <@@ new DbPediaOntologyTypeBase( ((%%(args.[0]) : DbPediaDataContextBase) :> IDbPediaObject).Connection, "Ontology" ) @@> ))
            ontologyProperty.AddXmlDoc("The DBpedia Ontology is a shallow, cross-domain ontology, which has been manually created based on the most commonly used infoboxes within Wikipedia. The ontology currently covers 529 classes which form a subsumption hierarchy and are described by 2,333 different properties.")
            let categoryProperty = 
                ProvidedProperty("Wikipedia Categories", categoriesType, 
                    GetterCode= (fun args -> <@@ new DbPediaCategoryTypeBase( ((%%(args.[0]) : DbPediaDataContextBase) :> IDbPediaObject).Connection, "Main topic categories" ) @@> ))
            categoryProperty.AddXmlDoc("These Wikipedia categories are represented using SKOS (Simple Knowledge Organization System), of which a hierarchical subset is presented here.")
            t.AddMember(ontologyProperty)
            t.AddMember(categoryProperty)
            typeFactory.GetServiceTypes().AddMember(t)
            t

        if sample = "" then
            // specific dbpedia resource is not specified, so start at the root
            instantiatedTy.AddMember(ProvidedMethod("GetDataContext", [], dbPediaDataContextType, IsStaticMethod=true, 
                                        InvokeCode= (fun _ -> <@@ new DbPediaDataContextBase(locale, limit) @@>)))    
        elif sample.StartsWith("http://dbpedia.org/resource/") then
            // specific dbpedia resource is specified, so resolve it and provide its type
            let label = sample.Replace("http://dbpedia.org/resource/", "").Replace("_", " ")
            ProvidedMethod("GetSample", [], typeFactory.MakeTypeForIndividual(sample), IsStaticMethod=true, 
                InvokeCode= (fun _ -> <@@ new DbPediaIndividualBase(new DbPediaConnection(locale, limit), sample, label) @@>))
            |> instantiatedTy.AddMember
        else
            raise <| ArgumentException("Static parameter 'Sample' expected a string that starts with 'http://dbpedia.org/resource/' but was given: " + sample)
        
        instantiatedTy.AddMember(typeFactory.GetServiceTypes())
        instantiatedTy

    let defaultLocale = "en"
    let defaultLimit = 2000
    let dbPediaType = createTypesForStaticParameters("DbPedia", defaultLocale, defaultLimit, "")

    let paramDbPediaType =
        let t = ProvidedTypeDefinition(thisAssembly, namespaceName, "DbPediaProvider", Some typeof<obj>)
        let sampleParam = ProvidedStaticParameter("Sample", typeof<string>, "")
        let langParam = ProvidedStaticParameter("locale", typeof<string>, defaultLocale)
        let limitParam = ProvidedStaticParameter("individualsLimit", typeof<int>, defaultLimit)
        t.DefineStaticParameters([langParam; limitParam; sampleParam], fun typeName parameterValues ->
            match parameterValues with
            | [| :? string as locale; :? int as limit; :? string as sample |] -> 
                createTypesForStaticParameters(typeName, locale, limit, sample)
            | _ -> failwith "unexpected static parameter values, expected (string, string, int)")
        t

    let searchDbPediaType =
        let t = ProvidedTypeDefinition(thisAssembly, namespaceName, "DbPediaSearch", Some typeof<obj>)
        let ontologyParam = ProvidedStaticParameter("ontologyClass", typeof<string>)
        let searchParam = ProvidedStaticParameter("searchTerm", typeof<string>)
        t.DefineStaticParameters([ontologyParam; searchParam], fun typeName parameterValues ->
            match parameterValues with
            | [| :? string as ontology; :? string as searchTerm |] -> 
                let schemaConn = DbPediaConnection(defaultLocale, defaultLimit)
                let typeFactory = DbPediaTypeFactory(schemaConn)

                let searchResultsType ont term =
                    let t = ProvidedTypeDefinition("SearchResultsType", Some typeof<DbPediaIndividualsTypeBase>, HideObjectMethods=true)
                    t.AddMembersDelayed(fun () -> (schemaConn.searchOntology ont term)
                                                  |> Array.map (fun e -> typeFactory.MakePropertyForIndividual e)
                                                  |> Array.toList)
                    typeFactory.GetServiceTypes().AddMember(t)
                    t
                
                let t = ProvidedTypeDefinition(thisAssembly, namespaceName, typeName, Some typeof<obj>)
                t.AddMember(ProvidedMethod("SearchResults", [], searchResultsType ontology searchTerm, IsStaticMethod=true,
                                           InvokeCode= (fun args -> 
                                                           let defaultLocaleValue = defaultLocale 
                                                           let defaultLimitValue = defaultLimit 
                                                           <@@ new DbPediaIndividualsTypeBase(new DbPediaConnection(defaultLocaleValue, defaultLimitValue)) @@> ) ))
                t.AddMember(typeFactory.GetServiceTypes())
                t
            | _ -> failwith "unexpected parameter values, expected (string, string)")
        t

    // add the root items to the namespace
    do this.AddNamespace(namespaceName, [dbPediaType; paramDbPediaType; searchDbPediaType])
    
[<assembly:TypeProviderAssembly>] 
do()
