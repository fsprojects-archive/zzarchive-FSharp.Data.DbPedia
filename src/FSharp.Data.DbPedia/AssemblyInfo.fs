namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FSharp.Data.DbPedia")>]
[<assembly: AssemblyProductAttribute("FSharp.Data.DbPedia")>]
[<assembly: AssemblyDescriptionAttribute("An F# type provider for DBpedia")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
