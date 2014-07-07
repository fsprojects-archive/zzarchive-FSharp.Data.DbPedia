#if INTERACTIVE
#r "./bin/Release/FSharp.Data.DbPedia.dll"
#r "./bin/Release/nunit.framework.dll"
#load "FsUnit.fs"
#else
module FSharp.Data.DbPedia.Tests
#endif

open FSharp.Data
open NUnit.Framework
open FsUnit

let data = DbPedia.GetDataContext()

//type DbPediaWithParams = DbPedia<"en", 2000>

[<Test>]
let ``Access single-value property from ontology``() =
    let value = data.Ontology.Activity.Game.Individuals.Backgammon.Name
    value |> should equal "Backgammon"

[<Test>]
let ``Access multi-value property from ontology``() =
    let value = data.Ontology.Activity.Game.Individuals.``Active Exploits``.sameAs
    value |> should equal [|"http://rdf.freebase.com/ns/m.02jsmr";
                            "http://www.wikidata.org/entity/Q4677504";
                            "http://yago-knowledge.org/resource/Active_Exploits"|]
