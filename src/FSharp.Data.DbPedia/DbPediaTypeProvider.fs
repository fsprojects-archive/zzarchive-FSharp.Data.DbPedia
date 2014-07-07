// Copyright (c) Microsoft Corporation 2005-2011.
// This sample code is provided "as is" without warranty of any kind. 
// We disclaim all warranties, either express or implied, including the 
// warranties of merchantability and fitness for a particular purpose. 

// This is a sample type provider. It provides 100 types, each containing various properties, 
// methods and nested types.
//
// This code is a sample for use in conjunction with the F# 3.0 Developer Preview release of September 2011.
//
// 1. Using the Provider
// 
//   To use this provider, open a separate instance of Visual Studio 11 and reference the provider
//   using #r, e.g.
//      #r @"bin\Debug\HelloWorldTypeProvider.dll"
//
//   Then look for the types under 
//      Samples.HelloWorldTypeProvider
//
// 2. Recompiling the Provider
//
//   Make sure you have exited all instances of Visual Studio and F# Interactive using the 
//   provider DLL before recompiling the provider.
//
// 3. Debugging the Provider
//
//   To debug this provider using 'print' statements, make a script that exposes a 
//   problem with the provider, then use
// 
//      fsc.exe -r:bin\Debug\HelloWorldTypeProvider.dll script.fsx
//
//   To debug this provider using Visual Studio, use
//
//      devenv.exe /debugexe fsc.exe -r:bin\Debug\HelloWorldTypeProvider.dll script.fsx
//
//   and disable "Just My Code" debugging. Consider setting first-chance exception catching using 
//
//      Debug --> Exceptions --> CLR Exceptions --> Thrown

namespace FSharp.Data.DbPediaTypeProvider

open System
open System.Reflection
open System.Collections.Generic
open Samples.FSharp.ProvidedTypes
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

type DbPediaDataContextBase(locale: string, limit: int) =
    let conn = DbPediaConnection(locale, limit)
    member __.Connection = conn

type DbPediaIndividualsTypeBase(conn: DbPediaConnection) =
    member __.Connection = conn

type DbPediaIndividualBase(conn: DbPediaConnection, uri, label) =
    member __.Connection = conn
    member __.Uri = uri
    member __.Name = label
    member __.Abstract = conn.tryGetPropertyValue<string> uri "http://dbpedia.org/ontology/abstract"
    member __.Thumbnail = conn.tryGetPropertyValue<string> uri "http://dbpedia.org/ontology/thumbnail"
    member __.GetProperties() = conn.getPropertyBag uri
    member __.GetPropertyValue(propertyUri) = conn.getPropertyValues uri propertyUri

type DbPediaOntologyTypeBase(conn: DbPediaConnection, uri: string) =
    let individuals = conn.getIndividuals uri
    let data = seq {for uri, label in individuals do
                        yield new DbPediaIndividualBase(conn, uri, label)}

    interface System.Collections.Generic.IEnumerable<DbPediaIndividualBase> with
        member __.GetEnumerator() = data.GetEnumerator()

    interface System.Collections.IEnumerable with
        member __.GetEnumerator() = data.GetEnumerator() :> System.Collections.IEnumerator

    member __.Connection = conn

// This defines the type provider. When compiled to a DLL it can be added as a reference to an F#
// command-line compilation, script or project.
[<TypeProvider>]
type DbPediaTypeProvider(config: TypeProviderConfig) as this = 

    // Inheriting from this type provides implementations of ITypeProvider in terms of the
    // provided types below.
    inherit TypeProviderForNamespaces()
    
    let namespaceName = "FSharp.Data"
    let thisAssembly = Assembly.GetExecutingAssembly()

    let defaultLocale = "en"
    let defaultLimit = 2000
    let schemaConn = DbPediaConnection(defaultLocale, defaultLimit)

    let serviceTypes =
        ProvidedTypeDefinition("ServiceTypes", Some typeof<obj>, HideObjectMethods=true)
     
    let makePropertyForPropertyOfIndividual (entityUri:string) propUri propName propType =
        ProvidedProperty(propName, propType, 
            GetterCode=  
                (fun args -> 
                        let connExpr = <@@ (%%(args.[0]) : DbPediaIndividualBase).Connection @@>
                        let minfo = 
                            if propType.IsArray then
                                let ty = propType.GetElementType() 
                                makeGetPropertyValuesHelper ty
                            else
                                makeGetPropertyValueHelper propType
                        Expr.Call(connExpr, minfo, [ <@@ entityUri @@>; <@@ propUri @@> ])))
                            
    let makeTypeForIndividual (uri : string) =
        let t = ProvidedTypeDefinition(uri.Replace("http://dbpedia.org/resource/", "") + "Type", Some typeof<DbPediaIndividualBase>, HideObjectMethods=true)
        t.AddMembersDelayed(fun () -> uri
                                      |> schemaConn.getPropertyBag
                                      |> List.map (fun (pUri, pLabel, pType) -> makePropertyForPropertyOfIndividual uri pUri pLabel pType))
        serviceTypes.AddMember(t)
        t
        
    let makePropertyForIndividual (uri, label) =
        let p = ProvidedProperty(label, makeTypeForIndividual uri, GetterCode= (fun args -> <@@ new DbPediaIndividualBase( (%%(args.[0]) : DbPediaIndividualsTypeBase).Connection, uri, label ) @@>  ))
        p

    let makeTypeForIndividuals (uri : string) =
        let t = ProvidedTypeDefinition(uri.Replace("http://dbpedia.org/ontology/", "") + "IndividualsType", Some typeof<DbPediaIndividualsTypeBase>, HideObjectMethods=true)
        t.AddMembersDelayed(fun () -> uri
                                      |> schemaConn.getIndividuals
                                      |> Array.map (fun e -> makePropertyForIndividual e) 
                                      |> Array.toList )
        serviceTypes.AddMember(t)
        t

    let rec makeTypeForOntologyType (uri : string) =
        let t = ProvidedTypeDefinition(uri.Replace("http://dbpedia.org/ontology/", "") + "Type", Some typeof<DbPediaOntologyTypeBase>, HideObjectMethods=true)
        t.AddMemberDelayed(fun () -> ProvidedProperty("Individuals", makeTypeForIndividuals uri, GetterCode= (fun args -> <@@ new DbPediaIndividualsTypeBase( (%%(args.[0]) : DbPediaOntologyTypeBase).Connection ) @@> )))
        t.AddMembersDelayed(fun () -> uri
                                      |> schemaConn.getOntologySubclasses
                                      |> Array.map (fun topic -> makePropertyForOntologyType topic) 
                                      |> Array.toList )
        serviceTypes.AddMember(t)
        t

    and makePropertyForOntologyType (uri, label) =
        ProvidedProperty(label, makeTypeForOntologyType uri, GetterCode= (fun args -> <@@ new DbPediaOntologyTypeBase( (%%(args.[0]) : DbPediaOntologyTypeBase).Connection, uri ) @@> ))    

    let ontologyType =
        let t = ProvidedTypeDefinition("OntologyType", Some typeof<DbPediaOntologyTypeBase>, HideObjectMethods=true)
        t.AddMembersDelayed(fun () -> "http://www.w3.org/2002/07/owl#Thing"
                                       |> schemaConn.getOntologySubclasses
                                       |> Array.map (fun topic -> makePropertyForOntologyType topic)
                                       |> Array.toList)
        serviceTypes.AddMember(t)
        t

    let dbPediaDataContextType =
        let t = ProvidedTypeDefinition("DbPediaDataContext", Some typeof<DbPediaDataContextBase>, HideObjectMethods=true)
        t.AddMember(ProvidedProperty("Ontology", ontologyType, 
                                     GetterCode= (fun args -> <@@ new DbPediaOntologyTypeBase( (%%(args.[0]) : DbPediaDataContextBase).Connection, "Ontology" ) @@> )))
        serviceTypes.AddMember(t)
        t

    let dbPediaType =
        let t = ProvidedTypeDefinition(thisAssembly, namespaceName, "DbPedia", Some typeof<obj>)
        let defaultLocaleValue = defaultLocale
        let defaultLimitValue = defaultLimit
        let langParam = ProvidedStaticParameter("language", typeof<string>, defaultLocaleValue)
        let limitParam = ProvidedStaticParameter("individualsLimit", typeof<int>, defaultLimitValue)

        t.AddMember(ProvidedMethod("GetDataContext", [], dbPediaDataContextType, IsStaticMethod=true, 
                                   InvokeCode= (fun _ -> <@@ new DbPediaDataContextBase(defaultLocaleValue, defaultLimitValue) @@>)))
        t.AddMember(serviceTypes)
        t.DefineStaticParameters([langParam; limitParam], (fun typeName parameterValues ->
            match parameterValues with
            | [| :? string as language; :? int as limit |] ->
                let t = ProvidedTypeDefinition(thisAssembly, namespaceName, "DbPedia", Some typeof<obj>)
                t.AddMember(ProvidedMethod("GetDataContext", [], dbPediaDataContextType, IsStaticMethod=true, 
                                   InvokeCode= (fun _ -> <@@ new DbPediaDataContextBase(language, limit) @@>)))
                t
            | _ -> failwith "unexpected parameter values, expected (string, int)"))
        t
    
    // add the root item to the namespace
    do this.AddNamespace(namespaceName, [dbPediaType])
                            
[<assembly:TypeProviderAssembly>] 
do()
