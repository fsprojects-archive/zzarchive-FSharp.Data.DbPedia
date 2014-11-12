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

#r "FSharp.Data.DbPedia.dll"
open FSharp.Data

let data = DbPedia.GetDataContext()

(** 
 
### Exploring the DBpedia ontology
 
Many entities in DBpedia are organized into a 
[hierarchical ontology](http://mappings.dbpedia.org/server/ontology/classes/)
of topics which can be accessed by typing `data.Ontology.` and exploring the 
available data sources in the autocomplete. For example, the following snippet 
retrieves scientists and then looks at the details of Albert Einstein: 
*) 

let scientists = data.Ontology.Agent.Person.Scientist.Individuals
scientists.``Albert Einstein``.dateOfBirth      // returns 1879
scientists.``Albert Einstein``.residence        // returns "Germany, Italy, Switzerland, Austria, Belgium, United States"

(**

#### Internal DBpedia links

Sometimes an individual's property is a URL link instead of raw data. These URLs can be external
links (e.g. to a [Freebase](http://freebase.com) entity), but often they are links to other DBpedia
entities. The DBpedia type provider automatically recognizes these links and provides a helpful
`(dbpedia ref)` annotation to such properties.

![](/FSharp.Data.DbPedia/img/dbpediaref.png)

These properties can be expanded to reveal further structure and properties.
*)

scientists.``Albert Einstein``.``academicAdvisor (dbpedia ref)``.givenName      // returns "Heinrich Friedrich"
scientists.``Albert Einstein``.``academicAdvisor (dbpedia ref)``.``almaMater (dbpedia ref)``.established    // 1558

(**

### Searching for Individuals

There are several options to quickly find an individual in DBpedia's immense ontology.
The first is to use `.Individuals.` on any ontology topic (as we did with scientists above)
and rely on the editor's intellisense to automatically filter members as you start typing a name.

The second option is to use `.IndividualsAZ` where the individuals are organized into 26 buckets
corresponding to their first letter. This can be helpful when the number of individuals exceeds
the maximum limit retrieved at a time (default 2000) causing your desired individual to not appear.
*)

data.Ontology.CelestialBody.Planet.Individuals              // Saturn doesn't appear here
data.Ontology.CelestialBody.Planet.IndividualsAZ.S.Saturn   // there it is!

(**

The third option is to use a special pair of static parameters to specify:

1.  The [ontology topic](http://mappings.dbpedia.org/server/ontology/classes/) to search within
2.  A search term

Once these parameters are defined, calling the method `SearchResults()` will 
provide a list of individuals in the ontology topic which contain the search term.

![](/FSharp.Data.DbPedia/img/search.png)

Using a more specific ontology subtopic (e.g. replacing `"Place"` with `"City"` above) may help to
narrow down the possibilities and provide more relevant search results for your query.

### Units of Measure 

Units of measure are supported. For example, the `averageSpeed` property of Saturn 
is provided in kilometres per second. For any SI units, the F# core library 
`UnitSystems.SI` units are used. Otherwise units from `DbPediaMeasures` is used. Units of
measure information is only available at compile time, so the types of these values
will simply be `float` at runtime.
 
Here is an example from data about the planet Saturn: 
*) 

let saturn = data.Ontology.CelestialBody.Planet.IndividualsAZ.S.Saturn
let avgSpeed = saturn.averageSpeed      // mouse-over 'avgSpeed' to see its type
let density = saturn.density
let minDistanceFromSun = saturn.minimumDistanceFromSun