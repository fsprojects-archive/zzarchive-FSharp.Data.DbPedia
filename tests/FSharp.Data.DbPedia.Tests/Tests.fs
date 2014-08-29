#if INTERACTIVE
#r "bin/Release/FSharp.Data.dll"
#r "../../bin/FSharp.Data.DbPedia.dll"
#r "./bin/Release/nunit.framework.dll"
#load "FsUnit.fs"
#else
module FSharp.Data.DbPedia.Tests
#endif

open FSharp.Data
open NUnit.Framework
open FsUnit

let showImage (url : string) =
    System.Diagnostics.Process.Start(url)

let bingMaps (coord : DbPediaAccess.Geography) =
    System.Diagnostics.Process.Start(sprintf "http://bing.com/maps/default.aspx?cp=%f~%f&lvl=16" coord.latitude coord.longitude)

let data = FSharp.Data.DbPedia.GetDataContext()
let einstein = data.Ontology.Agent.Person.Scientist.Individuals.``Albert Einstein``
einstein.``abstract``

let massOfEarth = data.Ontology.CelestialBody.Planet.IndividualsAZ.E.Earth
//let massOfMoon = data.Ontology.CelestialBody.Planet.IndividualsAZ.M.Moon.
//let massOfMercury = data.Ontology.CelestialBody.Planet.IndividualsAZ.M.``Mercury (planet)``.
//let massOfVenus = data.Ontology.CelestialBody.Planet.IndividualsAZ.V.Venus.mass

//let mercuryDistanceFromSun  = 57910000.0<km> * AU_per_km 
//let venusDistanceFromSun    = 0.723332<AU> 
//let distanceFromMoonToEarth =384403.0<km> * AU_per_km 
// 
//let orbitalSpeedOfMoon   = 1.023<km/s> * AU_per_km 
//let orbitalSpeedOfMecury = 47.87<km/s> * AU_per_km 
//let orbitalSpeedOfVenus  = 35.02<km/s> * AU_per_km 
//let orbitalSpeedOfEarth  = 29.8<km/s>  * AU_per_km  

type Sample = FSharp.Data.DbPediaProvider<Sample="http://dbpedia.org/resource/Albert_Einstein" >

//let searchData = FSharp.Data.DbPediaSearch<"Capital", "Paris">.SearchResults()

let q = query {for g in data.Ontology.Activity.Game do 
               select g }

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
