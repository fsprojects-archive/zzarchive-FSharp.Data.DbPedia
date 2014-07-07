(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(** 
# F# Data: DBpedia Provider


The [DBpedia graph database](http://www.dbpedia.org) is a "crowd-sourced community effort to
extract structured information from [Wikipedia](http://wikipedia.org) and make this information
available on the web." The English version of DBpedia describes 4.0 million things, out of which 
3.22 million are classified in a consistent ontology.

The DBpedia type provider puts this information at your fingertips, giving you strongly-typed
access to the extensive knowledge on Wikipedia. This article provides a brief introduction showing
some of the features. 


## Introducing the provider


The following example loads the `FSharp.Data.DbPedia.dll` library (in F# Interactive), 
initializes a connection to DBpedia using the `GetDataContext` method:
*)

#r "../../bin/FSharp.Data.DbPedia.dll"
open FSharp.Data

let data = DbPedia.GetDataContext()
