[![Issue Stats](http://issuestats.com/github/fsprojects/FSharp.Data.DbPedia/badge/issue)](http://issuestats.com/github/fsprojects/FSharp.Data.DbPedia)
[![Issue Stats](http://issuestats.com/github/fsprojects/FSharp.Data.DbPedia/badge/pr)](http://issuestats.com/github/fsprojects/FSharp.Data.DbPedia)

# F# DBpedia type provider [![NuGet Status](http://img.shields.io/nuget/v/FSharp.Data.DbPedia.svg?style=flat)](https://www.nuget.org/packages/FSharp.Data.DbPedia/)

The F# DBpedia type provider (`FSharp.Data.DbPedia.dll`) implements everything you need to access Wikipedia data in your F# applications 
and scripts.


See http://fsprojects.github.io/FSharp.Data.DbPedia/

See the [Developer Guide](DEVGUIDE.md) for information on building, testing and contributing to the project.

# Build Status

Head (branch ``master``), Windows Server 2012 (Appveyor)  [![Build status](https://ci.appveyor.com/api/projects/status/y6d9vcdeov9rqcgd/branch/master)](https://ci.appveyor.com/project/fsgit/fsharp-data-dbpedia)

Head (branch ``master``), OSX (Travis)  [![Build status](https://travis-ci.org/fsprojects/FSharp.Data.DbPedia.svg?branch=master)](https://travis-ci.org/fsprojects/FSharp.Data.DbPedia)





# Notes

* DbPedia often goes down for maintenance.  You won't be able to use the type provider, build tests or make a release while it's down.

* You can test whether [the DbPedia SPARQL JSON end point](http://dbpedia.org/sparql) is up
  by [running a sample query](http://dbpedia.org/sparql?default-graph-uri=http%3A%2F%2Fdbpedia.org&query=select+distinct+%3FConcept+where+%7B%5B%5D+a+%3FConcept%7D+LIMIT+100&format=application%2Fsparql-results%2Bjson&timeout=30000&debug=on)


## Maintainer(s)

- [@aastevenson](https://github.com/aastevenson)

The default maintainer account for projects under "fsprojects" is [@fsgit](https://github.com/fsgit) - F# Community Project Incubation Space (repo management)
