


To develop and build (debug mode):  open and use the solution FSharp.Data.DbPedia.sln

To test (debug mode):  F5 launch or open and use the solution tests/FSharp.Data.DbPedia.Tests/FSharp.Data.DbPedia.Tests.sln

To build and test (release mode):  build 

To build, test and build a release package:  build Release

To push the nuget package: .nuget\nuget.exe push bin\FSharp.Data.DbPedia.0.0.2.nupkg YOUR-NUGET-API-KEY

To build and push the docs: build ReleaseDocs

NOTES
-----

* DbPedia often goes down for maintenance.  You won't be able to use the type provider, build tests or make a release while it's down.

* You can test whether [the DbPedia SPARQL JSON end point]](http://dbpedia.org/sparql) is up
  by [running a sample query](http://dbpedia.org/sparql?default-graph-uri=http%3A%2F%2Fdbpedia.org&query=select+distinct+%3FConcept+where+%7B%5B%5D+a+%3FConcept%7D+LIMIT+100&format=application%2Fsparql-results%2Bjson&timeout=30000&debug=on)

