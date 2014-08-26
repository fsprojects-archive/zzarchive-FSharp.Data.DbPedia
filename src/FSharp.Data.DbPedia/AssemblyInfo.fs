namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FSharp.Data.DbPedia")>]
[<assembly: AssemblyProductAttribute("FSharp.Data.DbPedia")>]
[<assembly: AssemblyDescriptionAttribute("An F# type provider for DBpedia")>]
[<assembly: AssemblyVersionAttribute("0.0.2")>]
[<assembly: AssemblyFileVersionAttribute("0.0.2")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.2"
