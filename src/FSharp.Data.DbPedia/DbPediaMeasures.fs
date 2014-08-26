namespace DbPediaMeasures

open System.Globalization
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Samples.FSharp.ProvidedTypes

// Source: http://mappings.dbpedia.org/index.php/DBpedia_Datatypes

[<AutoOpen>]
module Length =
    [<Measure>] type kiloLightYear
    [<Measure>] type lightYear
    [<Measure>] type astronomicalUnit
    [<Measure>] type nauticalMile
    [<Measure>] type mile
    [<Measure>] type furlong
    [<Measure>] type chain
    [<Measure>] type rod
    [<Measure>] type fathom
    [<Measure>] type yard
    [<Measure>] type foot
    [<Measure>] type hand
    [<Measure>] type inch
    [<Measure>] type gigametre
    [<Measure>] type megametre
    [<Measure>] type kilometre
    [<Measure>] type hectometre
    [<Measure>] type decametre
    [<Measure>] type decimetre
    [<Measure>] type centimetre
    [<Measure>] type millimetre
    [<Measure>] type micrometre
    [<Measure>] type nanometre

[<AutoOpen>]
module Area =
    [<Measure>] type acre
    [<Measure>] type hectare
    [<Measure>] type valvetrain
    [<Measure>] type engineConfiguration
    [<Measure>] type fuelType

[<AutoOpen>]
module Mass =
    [<Measure>] type carat
    [<Measure>] type grain
    [<Measure>] type ounce
    [<Measure>] type pound
    [<Measure>] type stone
    [<Measure>] type tonne
    [<Measure>] type milligram
    [<Measure>] type gram

[<AutoOpen>]
module Time =
    [<Measure>] type year
    [<Measure>] type day
    [<Measure>] type hour
    [<Measure>] type minute

[<AutoOpen>]
module Temperature =
    [<Measure>] type degreeRankine
    [<Measure>] type degreeFahrenheit
    [<Measure>] type degreeCelcius

[<AutoOpen>]
module Volume =
    [<Measure>] type usGallon
    [<Measure>] type imperialGallon
    [<Measure>] type usBarrelOil
    [<Measure>] type imperialBarrelOil
    [<Measure>] type usBarrel
    [<Measure>] type imperialBarrel
    [<Measure>] type gigalitre
    [<Measure>] type megalitre
    [<Measure>] type kilolitre
    [<Measure>] type hectolitre
    [<Measure>] type decilitre
    [<Measure>] type centilitre
    [<Measure>] type millilitre
    [<Measure>] type microlitre
    [<Measure>] type litre

[<AutoOpen>]
module Force =
    [<Measure>] type poundal
    [<Measure>] type millipond
    [<Measure>] type milligramForce
    [<Measure>] type pond
    [<Measure>] type gramForce
    [<Measure>] type kilopond
    [<Measure>] type kilogramForce
    [<Measure>] type megapond
    [<Measure>] type tonneForce
    [<Measure>] type giganewton
    [<Measure>] type meganewton
    [<Measure>] type kilonewton
    [<Measure>] type millinewton
    [<Measure>] type nanonewton

[<AutoOpen>]
module Energy =
    [<Measure>] type megacalorie
    [<Measure>] type kilocalorie
    [<Measure>] type calorie
    [<Measure>] type millicalorie
    [<Measure>] type erg
    [<Measure>] type kilojoule

[<AutoOpen>]
module Power =
    [<Measure>] type brakehorsepower
    [<Measure>] type pferdestaerke
    [<Measure>] type horsepower
    [<Measure>] type terawatt
    [<Measure>] type gigawatt
    [<Measure>] type megawatt
    [<Measure>] type milliwatt
    [<Measure>] type kilowatt

[<AutoOpen>]
module Pressure =
    [<Measure>] type standardAtmosphere
    [<Measure>] type bar
    [<Measure>] type decibar
    [<Measure>] type millibar
    [<Measure>] type megapascal
    [<Measure>] type kilopascal
    [<Measure>] type hectopascal
    [<Measure>] type millipascal

[<AutoOpen>]
module Speed =
    [<Measure>] type knot

[<AutoOpen>]
module Frequency =
    [<Measure>] type terahertz
    [<Measure>] type gigahertz
    [<Measure>] type megahertz
    [<Measure>] type kilohertz
    [<Measure>] type millihertz

[<AutoOpen>]
module InformationUnit =
    [<Measure>] type terabyte
    [<Measure>] type gigabyte
    [<Measure>] type megabyte
    [<Measure>] type kilobyte
    [<Measure>] type megabit
    [<Measure>] type kilobit
    [<Measure>] type bit
    [<Measure>] type byte

[<AutoOpen>]
module Currency =
    [<Measure>] type zimbabweanDollar
    [<Measure>] type southAfricanRand
    [<Measure>] type yemenRial
    [<Measure>] type cfpFranc
    [<Measure>] type westAfricanCfaFranc
    [<Measure>] type eastCaribbeanDollar
    [<Measure>] type centralAfricanCfaFranc
    [<Measure>] type samoanTala
    [<Measure>] type vanuatuVatu
    [<Measure>] type venezuelanBolivar
    [<Measure>] type uruguayanPeso
    [<Measure>] type ugandaShilling
    [<Measure>] type ukranianHryvnia
    [<Measure>] type tanzanianShilling
    [<Measure>] type newTaiwanDollar
    [<Measure>] type trinidadAndTobagoDollar
    [<Measure>] type turkishLira
    [<Measure>] type tonganPaanga
    [<Measure>] type tunisianDinar
    [<Measure>] type azerbaijaniManat
    [<Measure>] type turkmenistaniManat
    [<Measure>] type tajikistaniSomoni
    [<Measure>] type thaiBaht
    [<Measure>] type swaziLilangeni
    [<Measure>] type syrianPound
    [<Measure>] type saoTomeAndPrincipeDobra
    [<Measure>] type surinamDollar
    [<Measure>] type somaliShilling
    [<Measure>] type sierraLeoneanLeone
    [<Measure>] type slovakKoruna
    [<Measure>] type saintHelenaPound
    [<Measure>] type singaporeDollar
    [<Measure>] type swedishKrona
    [<Measure>] type sudanesePound
    [<Measure>] type seychellesRupee
    [<Measure>] type solomonIslandsDollar 
    [<Measure>] type saudiRiyal 
    [<Measure>] type rwandaFranc 
    [<Measure>] type serbianDinar 
    [<Measure>] type romanianNewLeu
    [<Measure>] type qatariRial 
    [<Measure>] type paraguayanGuarani 
    [<Measure>] type polishZloty 
    [<Measure>] type pakistaniRupee 
    [<Measure>] type philippinePeso 
    [<Measure>] type papuaNewGuineanKina 
    [<Measure>] type peruvianNuevoSol 
    [<Measure>] type panamanianBalboa 
    [<Measure>] type omaniRial 
    [<Measure>] type newZealandDollar 
    [<Measure>] type nepaleseRupee 
    [<Measure>] type norwegianKrone 
    [<Measure>] type nicaraguanCordoba 
    [<Measure>] type nigerianNaira 
    [<Measure>] type namibianDollar 
    [<Measure>] type mozambicanMetical 
    [<Measure>] type malaysianRinggit 
    [<Measure>] type mexicanPeso 
    [<Measure>] type zambianKwacha 
    [<Measure>] type malawianKwacha 
    [<Measure>] type maldivianRufiyaa 
    [<Measure>] type mauritianRupee 
    [<Measure>] type mauritanianOuguiya 
    [<Measure>] type macanesePataca 
    [<Measure>] type mongolianTogrog 
    [<Measure>] type myanmaKyat 
    [<Measure>] type macedonianDenar 
    [<Measure>] type malagasyAriary 
    [<Measure>] type moldovanLeu 
    [<Measure>] type moroccanDirham 
    [<Measure>] type libyanDinar 
    [<Measure>] type latvianLats 
    [<Measure>] type lithuanianLitas 
    [<Measure>] type lesothoLoti 
    [<Measure>] type liberianDollar 
    [<Measure>] type sriLankanRupee 
    [<Measure>] type lebanesePound 
    [<Measure>] type laoKip 
    [<Measure>] type kazakhstaniTenge 
    [<Measure>] type caymanIslandsDollar 
    [<Measure>] type kuwaitiDinar 
    [<Measure>] type southKoreanWon 
    [<Measure>] type northKoreanWon 
    [<Measure>] type comorianFranc 
    [<Measure>] type cambodianRiel 
    [<Measure>] type uzbekistanSom 
    [<Measure>] type kyrgyzstaniSom 
    [<Measure>] type kenyanShilling 
    [<Measure>] type jordanianDinar 
    [<Measure>] type jamaicanDollar 
    [<Measure>] type icelandKrona 
    [<Measure>] type iranianRial 
    [<Measure>] type iraqiDinar 
    [<Measure>] type indianRupee 
    [<Measure>] type israeliNewSheqel 
    [<Measure>] type indonesianRupiah 
    [<Measure>] type hungarianForint 
    [<Measure>] type haitiGourde 
    [<Measure>] type croatianKuna 
    [<Measure>] type honduranLempira 
    [<Measure>] type hongKongDollar 
    [<Measure>] type guyanaDollar 
    [<Measure>] type guatemalanQuetzal 
    [<Measure>] type guineaFranc 
    [<Measure>] type gambianDalasi 
    [<Measure>] type gibraltarPound 
    [<Measure>] type ghanaianCedi 
    [<Measure>] type georgianLari 
    [<Measure>] type falklandIslandsPound 
    [<Measure>] type fijiDollar 
    [<Measure>] type ethiopianBirr 
    [<Measure>] type eritreanNakfa 
    [<Measure>] type egyptianPound 
    [<Measure>] type estonianKroon 
    [<Measure>] type algerianDinar 
    [<Measure>] type dominicanPeso 
    [<Measure>] type danishKrone 
    [<Measure>] type djiboutianFranc 
    [<Measure>] type czechKoruna 
    [<Measure>] type capeVerdeEscudo 
    [<Measure>] type cubanPeso 
    [<Measure>] type costaRicanColon 
    [<Measure>] type colombianPeso 
    [<Measure>] type renminbi 
    [<Measure>] type chileanPeso 
    [<Measure>] type swissFranc 
    [<Measure>] type congoleseFranc 
    [<Measure>] type canadianDollar 
    [<Measure>] type belizeDollar 
    [<Measure>] type belarussianRuble 
    [<Measure>] type botswanaPula 
    [<Measure>] type bhutaneseNgultrum 
    [<Measure>] type bahamianDollar 
    [<Measure>] type brazilianReal 
    [<Measure>] type bolivianBoliviano 
    [<Measure>] type bruneiDollar 
    [<Measure>] type bermudianDollar 
    [<Measure>] type burundianFranc 
    [<Measure>] type bahrainiDinar 
    [<Measure>] type bulgarianLev 
    [<Measure>] type bangladeshiTaka 
    [<Measure>] type barbadosDollar 
    [<Measure>] type bosniaAndHerzegovinaConvertibleMarks 
    [<Measure>] type arubanGuilder 
    [<Measure>] type australianDollar 
    [<Measure>] type argentinePeso 
    [<Measure>] type angolanKwanza 
    [<Measure>] type netherlandsAntilleanGuilder 
    [<Measure>] type armenianDram 
    [<Measure>] type albanianLek 
    [<Measure>] type afghanAfghani 
    [<Measure>] type unitedArabEmiratesDirham 
    [<Measure>] type russianRouble 
    [<Measure>] type japaneseYen 
    [<Measure>] type poundSterling 
    [<Measure>] type euro 
    [<Measure>] type usDollar 

module MeasureHelpers =
    type Samples.FSharp.ProvidedTypes.ProvidedMeasureBuilder with
        member this.Cube(m:System.Type) = this.Product(m, this.Product(m,m))

    [<Literal>]
    let CURRENCY_STYLE = NumberStyles.AllowDecimalPoint ||| NumberStyles.AllowExponent
       
    let mb = ProvidedMeasureBuilder.Default

    let (|DataType|) (uri:string) =
        let (|Ratio|_|) (dt:string) =
            match dt.Split([|"Per"|], System.StringSplitOptions.None) with
            | [|numerator; denominator|] -> Some(numerator, denominator)
            | _ -> None

        let (|Square|_|) (dt:string) =
            if dt.ToLower().StartsWith("square") then
                Some dt.[6..]
            else
                None

        let (|Cubic|_|) (dt:string) =
            if dt.ToLower().StartsWith("cubic") then
                Some dt.[5..]
            else
                None

        let (|Product|_|) (dt:string) =
            match dt with
            | "footPound" -> Some("foot", "pound")
            | "inchPound" -> Some("inch", "pound")
            | "poundFoot" -> Some("pound", "foot")
            | "terawattHour" -> Some("terawatt", "hour")
            | "gigawattHour" -> Some("gigawatt", "hour")
            | "megawattHour" -> Some("megawatt", "hour")
            | "kilowattHour" -> Some("kilowatt", "hour")
            | "wattHour" -> Some("watt", "hour")
            | "milliwattHour" -> Some("milliwatt", "hour")
            | "newtonCentimetre" -> Some("newton", "centimetre")
            | "newtonMillimetre" -> Some("newton", "millimetre")
            | "newtonMetre" -> Some("newton", "metre")
            | _ -> None
        
        let rec parseDataType (dt:string) =
            match dt with
            | Ratio(numerator, denominator) -> mb.Ratio(parseDataType numerator, parseDataType denominator)
            | Square(x) -> mb.Square(parseDataType x)
            | Cubic(x) -> mb.Cube(parseDataType x)
            | Product(a,b) -> mb.Product(parseDataType a, parseDataType b)
            | "inhabitants" -> mb.One
            | _ -> 
                let measureType = System.Reflection.Assembly.GetExecutingAssembly().GetTypes() |> Array.tryFind (fun ty -> ty.Name = dt.ToLower())
                match measureType with
                | Some ty -> ty                     // check the measures defined here first
                | None -> mb.SI(dt.ToLower())       // otherwise use SI measures in standard lib

        let dt = uri.Replace("http://dbpedia.org/datatype/", "")
        (fun (v:string) -> box(System.Double.Parse(v, CURRENCY_STYLE))), mb.AnnotateType(typeof<double>, [parseDataType dt])

(*  
        match uri with

        // area
        | "http://dbpedia.org/datatype/squareNauticalMile" -> fun (v:string) -> float v * 1.0<nauticalMile^2> |> box
        | "http://dbpedia.org/datatype/squareMile" -> fun v -> float v * 1.0<mile^2> |> box
        | "http://dbpedia.org/datatype/acre" -> fun v -> float v * 1.0<acre> |> box
        | "http://dbpedia.org/datatype/squareYard" -> fun v -> float v * 1.0<yard^2> |> box
        | "http://dbpedia.org/datatype/squareFoot" -> fun v -> float v * 1.0<foot^2> |> box
        | "http://dbpedia.org/datatype/squareInch" -> fun v -> float v * 1.0<inch^2> |> box
        | "http://dbpedia.org/datatype/hectare" -> fun v -> float v * 1.0<hectare> |> box
        | "http://dbpedia.org/datatype/squareKilometre" -> fun v -> float v * 1.0<kilometre^2> |> box
        | "http://dbpedia.org/datatype/squareHectometre" -> fun v -> float v * 1.0<hectometre^2> |> box
        | "http://dbpedia.org/datatype/squareDecametre" -> fun v -> float v * 1.0<decametre^2> |> box
        | "http://dbpedia.org/datatype/squareCentimetre" -> fun v -> float v * 1.0<centimetre^2> |> box
        | "http://dbpedia.org/datatype/squareMillimetre" -> fun v -> float v * 1.0<millimetre^2> |> box
        | "http://dbpedia.org/datatype/squareMetre" -> fun v -> float v * 1.0<metre^2> |> box
        | "http://dbpedia.org/datatype/valvetrain" -> fun v -> float v * 1.0<valvetrain> |> box
        | "http://dbpedia.org/datatype/engineConfiguration" -> fun v -> float v * 1.0<engineConfiguration> |> box
        | "http://dbpedia.org/datatype/fuelType" -> fun v -> float v * 1.0<fuelType> |> box
        
        // currency
        | "http://dbpedia.org/datatype/zimbabweanDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<zimbabweanDollar> |> box
        | "http://dbpedia.org/datatype/southAfricanRand" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<southAfricanRand> |> box
        | "http://dbpedia.org/datatype/yemenRial" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<yemenRial> |> box
        | "http://dbpedia.org/datatype/cfpFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<cfpFranc> |> box
        | "http://dbpedia.org/datatype/westAfricanCfaFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<westAfricanCfaFranc> |> box
        | "http://dbpedia.org/datatype/eastCaribbeanDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<eastCaribbeanDollar> |> box
        | "http://dbpedia.org/datatype/centralAfricanCfaFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<centralAfricanCfaFranc> |> box
        | "http://dbpedia.org/datatype/samoanTala" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<samoanTala> |> box
        | "http://dbpedia.org/datatype/vanuatuVatu" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<vanuatuVatu> |> box
        | "http://dbpedia.org/datatype/venezuelanBolivar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<venezuelanBolivar> |> box
        | "http://dbpedia.org/datatype/uruguayanPeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<uruguayanPeso> |> box
        | "http://dbpedia.org/datatype/ugandaShilling" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<ugandaShilling> |> box
        | "http://dbpedia.org/datatype/ukranianHryvnia" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<ukranianHryvnia> |> box
        | "http://dbpedia.org/datatype/tanzanianShilling" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<tanzanianShilling> |> box
        | "http://dbpedia.org/datatype/newTaiwanDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<newTaiwanDollar> |> box
        | "http://dbpedia.org/datatype/trinidadAndTobagoDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<trinidadAndTobagoDollar> |> box
        | "http://dbpedia.org/datatype/turkishLira" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<turkishLira> |> box
        | "http://dbpedia.org/datatype/tonganPaanga" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<tonganPaanga> |> box
        | "http://dbpedia.org/datatype/tunisianDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<tunisianDinar> |> box
        | "http://dbpedia.org/datatype/azerbaijaniManat" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<azerbaijaniManat> |> box
        | "http://dbpedia.org/datatype/turkmenistaniManat" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<turkmenistaniManat> |> box
        | "http://dbpedia.org/datatype/tajikistaniSomoni" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<tajikistaniSomoni> |> box
        | "http://dbpedia.org/datatype/thaiBaht" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<thaiBaht> |> box
        | "http://dbpedia.org/datatype/swaziLilangeni" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<swaziLilangeni> |> box
        | "http://dbpedia.org/datatype/syrianPound" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<syrianPound> |> box
        | "http://dbpedia.org/datatype/s%C3%A3oTom%C3%A9AndPrincipeDobra" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<saoTomeAndPrincipeDobra> |> box
        | "http://dbpedia.org/datatype/surinamDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<surinamDollar> |> box
        | "http://dbpedia.org/datatype/somaliShilling" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<somaliShilling> |> box
        | "http://dbpedia.org/datatype/sierraLeoneanLeone" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<sierraLeoneanLeone> |> box
        | "http://dbpedia.org/datatype/slovakKoruna" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<slovakKoruna> |> box
        | "http://dbpedia.org/datatype/saintHelenaPound" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<saintHelenaPound> |> box
        | "http://dbpedia.org/datatype/singaporeDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<singaporeDollar> |> box
        | "http://dbpedia.org/datatype/swedishKrona" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<swedishKrona> |> box
        | "http://dbpedia.org/datatype/sudanesePound" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<sudanesePound> |> box
        | "http://dbpedia.org/datatype/seychellesRupee" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<seychellesRupee> |> box
        | "http://dbpedia.org/datatype/solomonIslandsDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<solomonIslandsDollar> |> box
        | "http://dbpedia.org/datatype/saudiRiyal" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<saudiRiyal> |> box
        | "http://dbpedia.org/datatype/rwandaFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<rwandaFranc> |> box
        | "http://dbpedia.org/datatype/serbianDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<serbianDinar> |> box
        | "http://dbpedia.org/datatype/romanianNewLeu" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<romanianNewLeu> |> box
        | "http://dbpedia.org/datatype/qatariRial" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<qatariRial> |> box
        | "http://dbpedia.org/datatype/paraguayanGuarani" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<paraguayanGuarani> |> box
        | "http://dbpedia.org/datatype/polishZ%3Foty" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<polishZloty> |> box
        | "http://dbpedia.org/datatype/pakistaniRupee" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<pakistaniRupee> |> box
        | "http://dbpedia.org/datatype/philippinePeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<philippinePeso> |> box
        | "http://dbpedia.org/datatype/papuaNewGuineanKina" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<papuaNewGuineanKina> |> box
        | "http://dbpedia.org/datatype/peruvianNuevoSol" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<peruvianNuevoSol> |> box
        | "http://dbpedia.org/datatype/panamanianBalboa" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<panamanianBalboa> |> box
        | "http://dbpedia.org/datatype/omaniRial" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<omaniRial> |> box
        | "http://dbpedia.org/datatype/newZealandDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<newZealandDollar> |> box
        | "http://dbpedia.org/datatype/nepaleseRupee" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<nepaleseRupee> |> box
        | "http://dbpedia.org/datatype/norwegianKrone" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<norwegianKrone> |> box
        | "http://dbpedia.org/datatype/nicaraguanC%C3%B3rdoba" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<nicaraguanCordoba> |> box
        | "http://dbpedia.org/datatype/nigerianNaira" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<nigerianNaira> |> box
        | "http://dbpedia.org/datatype/namibianDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<namibianDollar> |> box
        | "http://dbpedia.org/datatype/mozambicanMetical" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<mozambicanMetical> |> box
        | "http://dbpedia.org/datatype/malaysianRinggit" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<malaysianRinggit> |> box
        | "http://dbpedia.org/datatype/mexicanPeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<mexicanPeso> |> box
        | "http://dbpedia.org/datatype/zambianKwacha" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<zambianKwacha> |> box
        | "http://dbpedia.org/datatype/malawianKwacha" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<malawianKwacha> |> box
        | "http://dbpedia.org/datatype/maldivianRufiyaa" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<maldivianRufiyaa> |> box
        | "http://dbpedia.org/datatype/mauritianRupee" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<mauritianRupee> |> box
        | "http://dbpedia.org/datatype/mauritanianOuguiya" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<mauritanianOuguiya> |> box
        | "http://dbpedia.org/datatype/macanesePataca" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<macanesePataca> |> box
        | "http://dbpedia.org/datatype/mongolianT%C3%B6gr%C3%B6g" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<mongolianTogrog> |> box
        | "http://dbpedia.org/datatype/myanmaKyat" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<myanmaKyat> |> box
        | "http://dbpedia.org/datatype/macedonianDenar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<macedonianDenar> |> box
        | "http://dbpedia.org/datatype/malagasyAriary" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<malagasyAriary> |> box
        | "http://dbpedia.org/datatype/moldovanLeu" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<moldovanLeu> |> box
        | "http://dbpedia.org/datatype/moroccanDirham" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<moroccanDirham> |> box
        | "http://dbpedia.org/datatype/libyanDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<libyanDinar> |> box
        | "http://dbpedia.org/datatype/latvianLats" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<latvianLats> |> box
        | "http://dbpedia.org/datatype/lithuanianLitas" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<lithuanianLitas> |> box
        | "http://dbpedia.org/datatype/lesothoLoti" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<lesothoLoti> |> box
        | "http://dbpedia.org/datatype/liberianDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<liberianDollar> |> box
        | "http://dbpedia.org/datatype/sriLankanRupee" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<sriLankanRupee> |> box
        | "http://dbpedia.org/datatype/lebanesePound" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<lebanesePound> |> box
        | "http://dbpedia.org/datatype/laoKip" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<laoKip> |> box
        | "http://dbpedia.org/datatype/kazakhstaniTenge" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<kazakhstaniTenge> |> box
        | "http://dbpedia.org/datatype/caymanIslandsDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<caymanIslandsDollar> |> box
        | "http://dbpedia.org/datatype/kuwaitiDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<kuwaitiDinar> |> box
        | "http://dbpedia.org/datatype/southKoreanWon" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<southKoreanWon> |> box
        | "http://dbpedia.org/datatype/northKoreanWon" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<northKoreanWon> |> box
        | "http://dbpedia.org/datatype/comorianFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<comorianFranc> |> box
        | "http://dbpedia.org/datatype/cambodianRiel" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<cambodianRiel> |> box
        | "http://dbpedia.org/datatype/uzbekistanSom" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<uzbekistanSom> |> box
        | "http://dbpedia.org/datatype/kyrgyzstaniSom" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<kyrgyzstaniSom> |> box
        | "http://dbpedia.org/datatype/kenyanShilling" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<kenyanShilling> |> box
        | "http://dbpedia.org/datatype/jordanianDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<jordanianDinar> |> box
        | "http://dbpedia.org/datatype/jamaicanDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<jamaicanDollar> |> box
        | "http://dbpedia.org/datatype/icelandKrona" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<icelandKrona> |> box
        | "http://dbpedia.org/datatype/iranianRial" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<iranianRial> |> box
        | "http://dbpedia.org/datatype/iraqiDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<iraqiDinar> |> box
        | "http://dbpedia.org/datatype/indianRupee" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<indianRupee> |> box
        | "http://dbpedia.org/datatype/israeliNewSheqel" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<israeliNewSheqel> |> box
        | "http://dbpedia.org/datatype/indonesianRupiah" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<indonesianRupiah> |> box
        | "http://dbpedia.org/datatype/hungarianForint" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<hungarianForint> |> box
        | "http://dbpedia.org/datatype/haitiGourde" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<haitiGourde> |> box
        | "http://dbpedia.org/datatype/croatianKuna" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<croatianKuna> |> box
        | "http://dbpedia.org/datatype/honduranLempira" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<honduranLempira> |> box
        | "http://dbpedia.org/datatype/hongKongDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<hongKongDollar> |> box
        | "http://dbpedia.org/datatype/guyanaDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<guyanaDollar> |> box
        | "http://dbpedia.org/datatype/guatemalanQuetzal" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<guatemalanQuetzal> |> box
        | "http://dbpedia.org/datatype/guineaFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<guineaFranc> |> box
        | "http://dbpedia.org/datatype/gambianDalasi" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<gambianDalasi> |> box
        | "http://dbpedia.org/datatype/gibraltarPound" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<gibraltarPound> |> box
        | "http://dbpedia.org/datatype/ghanaianCedi" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<ghanaianCedi> |> box
        | "http://dbpedia.org/datatype/georgianLari" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<georgianLari> |> box
        | "http://dbpedia.org/datatype/falklandIslandsPound" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<falklandIslandsPound> |> box
        | "http://dbpedia.org/datatype/fijiDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<fijiDollar> |> box
        | "http://dbpedia.org/datatype/ethiopianBirr" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<ethiopianBirr> |> box
        | "http://dbpedia.org/datatype/eritreanNakfa" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<eritreanNakfa> |> box
        | "http://dbpedia.org/datatype/egyptianPound" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<egyptianPound> |> box
        | "http://dbpedia.org/datatype/estonianKroon" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<estonianKroon> |> box
        | "http://dbpedia.org/datatype/algerianDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<algerianDinar> |> box
        | "http://dbpedia.org/datatype/dominicanPeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<dominicanPeso> |> box
        | "http://dbpedia.org/datatype/danishKrone" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<danishKrone> |> box
        | "http://dbpedia.org/datatype/djiboutianFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<djiboutianFranc> |> box
        | "http://dbpedia.org/datatype/czechKoruna" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<czechKoruna> |> box
        | "http://dbpedia.org/datatype/capeVerdeEscudo" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<capeVerdeEscudo> |> box
        | "http://dbpedia.org/datatype/cubanPeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<cubanPeso> |> box
        | "http://dbpedia.org/datatype/costaRicanColon" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<costaRicanColon> |> box
        | "http://dbpedia.org/datatype/colombianPeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<colombianPeso> |> box
        | "http://dbpedia.org/datatype/renminbi" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<renminbi> |> box
        | "http://dbpedia.org/datatype/chileanPeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<chileanPeso> |> box
        | "http://dbpedia.org/datatype/swissFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<swissFranc> |> box
        | "http://dbpedia.org/datatype/congoleseFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<congoleseFranc> |> box
        | "http://dbpedia.org/datatype/canadianDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<canadianDollar> |> box
        | "http://dbpedia.org/datatype/belizeDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<belizeDollar> |> box
        | "http://dbpedia.org/datatype/belarussianRuble" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<belarussianRuble> |> box
        | "http://dbpedia.org/datatype/botswanaPula" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<botswanaPula> |> box
        | "http://dbpedia.org/datatype/bhutaneseNgultrum" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bhutaneseNgultrum> |> box
        | "http://dbpedia.org/datatype/bahamianDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bahamianDollar> |> box
        | "http://dbpedia.org/datatype/brazilianReal" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<brazilianReal> |> box
        | "http://dbpedia.org/datatype/bolivianBoliviano" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bolivianBoliviano> |> box
        | "http://dbpedia.org/datatype/bruneiDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bruneiDollar> |> box
        | "http://dbpedia.org/datatype/bermudianDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bermudianDollar> |> box
        | "http://dbpedia.org/datatype/burundianFranc" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<burundianFranc> |> box
        | "http://dbpedia.org/datatype/bahrainiDinar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bahrainiDinar> |> box
        | "http://dbpedia.org/datatype/bulgarianLev" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bulgarianLev> |> box
        | "http://dbpedia.org/datatype/bangladeshiTaka" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bangladeshiTaka> |> box
        | "http://dbpedia.org/datatype/barbadosDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<barbadosDollar> |> box
        | "http://dbpedia.org/datatype/bosniaAndHerzegovinaConvertibleMarks" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<bosniaAndHerzegovinaConvertibleMarks> |> box
        | "http://dbpedia.org/datatype/arubanGuilder" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<arubanGuilder> |> box
        | "http://dbpedia.org/datatype/australianDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<australianDollar> |> box
        | "http://dbpedia.org/datatype/argentinePeso" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<argentinePeso> |> box
        | "http://dbpedia.org/datatype/angolanKwanza" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<angolanKwanza> |> box
        | "http://dbpedia.org/datatype/netherlandsAntilleanGuilder" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<netherlandsAntilleanGuilder> |> box
        | "http://dbpedia.org/datatype/armenianDram" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<armenianDram> |> box
        | "http://dbpedia.org/datatype/albanianLek" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<albanianLek> |> box
        | "http://dbpedia.org/datatype/afghanAfghani" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<afghanAfghani> |> box
        | "http://dbpedia.org/datatype/unitedArabEmiratesDirham" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<unitedArabEmiratesDirham> |> box
        | "http://dbpedia.org/datatype/russianRouble" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<russianRouble> |> box
        | "http://dbpedia.org/datatype/japaneseYen" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<japaneseYen> |> box
        | "http://dbpedia.org/datatype/poundSterling" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<poundSterling> |> box
        | "http://dbpedia.org/datatype/euro" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<euro> |> box
        | "http://dbpedia.org/datatype/usDollar" -> fun v -> int (System.Int64.Parse(v, CURRENCY_STYLE)) * 1<usDollar> |> box
        
        // density
        | "http://dbpedia.org/datatype/gramPerMillilitre" -> fun v -> float v * 1.0<gram/millilitre> |> box
        | "http://dbpedia.org/datatype/gramPerCubicCentimetre" -> fun v -> float v * 1.0<gram/centimetre^3> |> box
        | "http://dbpedia.org/datatype/kilogramPerLitre" -> fun v -> float v * 1.0<kilogram/litre> |> box
        | "http://dbpedia.org/datatype/kilogramPerCubicMetre" -> fun v -> float v * 1.0<kilogram/metre^3> |> box

        // energy
        | "http://dbpedia.org/datatype/footPound" -> fun v -> float v * 1.0<foot pound> |> box
        | "http://dbpedia.org/datatype/inchPound" -> fun v -> float v * 1.0<inch pound> |> box
        | "http://dbpedia.org/datatype/megacalorie" -> fun v -> float v * 1.0<megacalorie> |> box
        | "http://dbpedia.org/datatype/kilocalorie" -> fun v -> float v * 1.0<kilocalorie> |> box
        | "http://dbpedia.org/datatype/calorie" -> fun v -> float v * 1.0<calorie> |> box
        | "http://dbpedia.org/datatype/millicalorie" -> fun v -> float v * 1.0<millicalorie> |> box
        | "http://dbpedia.org/datatype/terawattHour" -> fun v -> float v * 1.0<terawatt hour> |> box
        | "http://dbpedia.org/datatype/gigawattHour" -> fun v -> float v * 1.0<gigawatt hour> |> box
        | "http://dbpedia.org/datatype/megawattHour" -> fun v -> float v * 1.0<megawatt hour> |> box
        | "http://dbpedia.org/datatype/kilowattHour" -> fun v -> float v * 1.0<kilowatt hour> |> box
        | "http://dbpedia.org/datatype/wattHour" -> fun v -> float v * 1.0<watt hour> |> box
        | "http://dbpedia.org/datatype/milliwattHour" -> fun v -> float v * 1.0<milliwatt hour> |> box
        | "http://dbpedia.org/datatype/erg" -> fun v -> float v * 1.0<erg> |> box
        | "http://dbpedia.org/datatype/kilojoule" -> fun v -> float v * 1.0<kilojoule> |> box
        | "http://dbpedia.org/datatype/joule" -> fun v -> float v * 1.0<joule> |> box

        // flow rate
        | "http://dbpedia.org/datatype/cubicFeetPerYear" -> fun v -> float v * 1.0<foot^3/year> |> box
        | "http://dbpedia.org/datatype/cubicMetrePerYear" -> fun v -> float v * 1.0<metre^3/year> |> box
        | "http://dbpedia.org/datatype/cubicFeetPerSecond" -> fun v -> float v * 1.0<foot^3/second> |> box
        | "http://dbpedia.org/datatype/cubicMetrePerSecond" -> fun v -> float v * 1.0<metre^3/second> |> box

        // force
        | "http://dbpedia.org/datatype/poundal" -> fun v -> float v * 1.0<poundal> |> box
        | "http://dbpedia.org/datatype/millipond" -> fun v -> float v * 1.0<millipond> |> box
        | "http://dbpedia.org/datatype/milligramForce" -> fun v -> float v * 1.0<milligramForce> |> box
        | "http://dbpedia.org/datatype/pond" -> fun v -> float v * 1.0<pond> |> box
        | "http://dbpedia.org/datatype/gramForce" -> fun v -> float v * 1.0<gramForce> |> box
        | "http://dbpedia.org/datatype/kilopond" -> fun v -> float v * 1.0<kilopond> |> box
        | "http://dbpedia.org/datatype/kilogramForce" -> fun v -> float v * 1.0<kilogramForce> |> box
        | "http://dbpedia.org/datatype/megapond" -> fun v -> float v * 1.0<megapond> |> box
        | "http://dbpedia.org/datatype/tonneForce" -> fun v -> float v * 1.0<tonneForce> |> box
        | "http://dbpedia.org/datatype/giganewton" -> fun v -> float v * 1.0<giganewton> |> box
        | "http://dbpedia.org/datatype/meganewton" -> fun v -> float v * 1.0<meganewton> |> box
        | "http://dbpedia.org/datatype/kilonewton" -> fun v -> float v * 1.0<kilonewton> |> box
        | "http://dbpedia.org/datatype/millinewton" -> fun v -> float v * 1.0<millinewton> |> box
        | "http://dbpedia.org/datatype/nanonewton" -> fun v -> float v * 1.0<nanonewton> |> box
        | "http://dbpedia.org/datatype/newton" -> fun v -> float v * 1.0<newton> |> box

        // fuel efficiency
        | "http://dbpedia.org/datatype/kilometresPerLitre" -> fun v -> float v * 1.0<kilometre/litre> |> box

        // frequency
        | "http://dbpedia.org/datatype/terahertz" -> fun v -> float v * 1.0<terahertz> |> box
        | "http://dbpedia.org/datatype/gigahertz" -> fun v -> float v * 1.0<gigahertz> |> box
        | "http://dbpedia.org/datatype/megahertz" -> fun v -> float v * 1.0<megahertz> |> box
        | "http://dbpedia.org/datatype/kilohertz" -> fun v -> float v * 1.0<kilohertz> |> box
        | "http://dbpedia.org/datatype/millihertz" -> fun v -> float v * 1.0<millihertz> |> box
        | "http://dbpedia.org/datatype/hertz" -> fun v -> float v * 1.0<hertz> |> box

        // information unit
        | "http://dbpedia.org/datatype/terabyte" -> fun v -> float v * 1.0<terabyte> |> box
        | "http://dbpedia.org/datatype/gigabyte" -> fun v -> float v * 1.0<gigabyte> |> box
        | "http://dbpedia.org/datatype/megabyte" -> fun v -> float v * 1.0<megabyte> |> box
        | "http://dbpedia.org/datatype/kilobit" -> fun v -> float v * 1.0<kilobit> |> box
        | "http://dbpedia.org/datatype/bit" -> fun v -> int v * 1<bit> |> box
        | "http://dbpedia.org/datatype/byte" -> fun v -> int v * 1<byte> |> box

        // length
        | "http://dbpedia.org/datatype/kiloLightYear" -> fun v -> float v * 1.0<kiloLightYear> |> box
        | "http://dbpedia.org/datatype/lightYear" -> fun v -> float v * 1.0<lightYear> |> box
        | "http://dbpedia.org/datatype/astronomicalUnit" -> fun v -> float v * 1.0<astronomicalUnit> |> box
        | "http://dbpedia.org/datatype/nauticalMile" -> fun v -> float v * 1.0<nauticalMile> |> box
        | "http://dbpedia.org/datatype/mile" -> fun v -> float v * 1.0<mile> |> box
        | "http://dbpedia.org/datatype/furlong" -> fun v -> float v * 1.0<furlong> |> box
        | "http://dbpedia.org/datatype/chain" -> fun v -> float v * 1.0<chain> |> box
        | "http://dbpedia.org/datatype/rod" -> fun v -> float v * 1.0<rod> |> box
        | "http://dbpedia.org/datatype/fathom" -> fun v -> float v * 1.0<fathom> |> box
        | "http://dbpedia.org/datatype/yard" -> fun v -> float v * 1.0<yard> |> box
        | "http://dbpedia.org/datatype/foot" -> fun v -> float v * 1.0<foot> |> box
        | "http://dbpedia.org/datatype/hand" -> fun v -> float v * 1.0<hand> |> box
        | "http://dbpedia.org/datatype/inch" -> fun v -> float v * 1.0<inch> |> box
        | "http://dbpedia.org/datatype/gigametre" -> fun v -> float v * 1.0<gigametre> |> box
        | "http://dbpedia.org/datatype/megametre" -> fun v -> float v * 1.0<megametre> |> box
        | "http://dbpedia.org/datatype/kilometre" -> fun v -> float v * 1.0<kilometre> |> box
        | "http://dbpedia.org/datatype/hectometre" -> fun v -> float v * 1.0<hectometre> |> box
        | "http://dbpedia.org/datatype/decametre" -> fun v -> float v * 1.0<decametre> |> box
        | "http://dbpedia.org/datatype/decimetre" -> fun v -> float v * 1.0<decimetre> |> box
        | "http://dbpedia.org/datatype/centimetre" -> fun v -> float v * 1.0<centimetre> |> box
        | "http://dbpedia.org/datatype/millimetre" -> fun v -> float v * 1.0<millimetre> |> box
        | "http://dbpedia.org/datatype/micometre" -> fun v -> float v * 1.0<micrometre> |> box
        | "http://dbpedia.org/datatype/nanometre" -> fun v -> float v * 1.0<nanometre> |> box
        | "http://dbpedia.org/datatype/metre" -> fun v -> float v * 1.0<metre> |> box

        // linear mass density
        | "http://dbpedia.org/datatype/gramPerKilometre" -> fun v -> float v * 1.0<gram/kilometre> |> box

        // mass
        | "http://dbpedia.org/datatype/carat" -> fun v -> float v * 1.0<carat> |> box
        | "http://dbpedia.org/datatype/grain" -> fun v -> float v * 1.0<grain> |> box
        | "http://dbpedia.org/datatype/ounce" -> fun v -> float v * 1.0<ounce> |> box
        | "http://dbpedia.org/datatype/pound" -> fun v -> float v * 1.0<pound> |> box
        | "http://dbpedia.org/datatype/stone" -> fun v -> float v * 1.0<stone> |> box
        | "http://dbpedia.org/datatype/tonne" -> fun v -> float v * 1.0<tonne> |> box
        | "http://dbpedia.org/datatype/kilogram" -> fun v -> float v * 1.0<kilogram> |> box
        | "http://dbpedia.org/datatype/milligram" -> fun v -> float v * 1.0<milligram> |> box
        | "http://dbpedia.org/datatype/gram" -> fun v -> float v * 1.0<gram> |> box

        // population density
        | "http://dbpedia.org/datatype/inhabitantsPerSquareMile" -> fun v -> double v * 1.0<1/mile^2> |> box
        | "http://dbpedia.org/datatype/inhabitantsPerSquareKilometre" -> fun v -> double v * 1.0<1/kilometre^2> |> box

        // power
        | "http://dbpedia.org/datatype/brake_horsepower" -> fun v -> float v * 1.0<brakehorsepower> |> box
        | "http://dbpedia.org/datatype/pferdestaerke" -> fun v -> float v * 1.0<pferdestaerke> |> box
        | "http://dbpedia.org/datatype/horsepower" -> fun v -> float v * 1.0<horsepower> |> box
        | "http://dbpedia.org/datatype/gigawatt" -> fun v -> float v * 1.0<gigawatt> |> box
        | "http://dbpedia.org/datatype/megawatt" -> fun v -> float v * 1.0<megawatt> |> box
        | "http://dbpedia.org/datatype/milliwatt" -> fun v -> float v * 1.0<milliwatt> |> box
        | "http://dbpedia.org/datatype/kilowatt" -> fun v -> float v * 1.0<kilowatt> |> box
        | "http://dbpedia.org/datatype/watt" -> fun v -> float v * 1.0<watt> |> box

        // pressure
        | "http://dbpedia.org/datatype/poundPerSquareInch" -> fun v -> float v * 1.0<pound/inch^2> |> box
        | "http://dbpedia.org/datatype/standardAtmosphere" -> fun v -> float v * 1.0<standardAtmosphere> |> box
        | "http://dbpedia.org/datatype/bar" -> fun v -> float v * 1.0<bar> |> box
        | "http://dbpedia.org/datatype/decibar" -> fun v -> float v * 1.0<decibar> |> box
        | "http://dbpedia.org/datatype/millibar" -> fun v -> float v * 1.0<millibar> |> box
        | "http://dbpedia.org/datatype/megapascal" -> fun v -> float v * 1.0<megapascal> |> box
        | "http://dbpedia.org/datatype/kilopascal" -> fun v -> float v * 1.0<kilopascal> |> box
        | "http://dbpedia.org/datatype/hectopascal" -> fun v -> float v * 1.0<hectopascal> |> box
        | "http://dbpedia.org/datatype/millipascal" -> fun v -> float v * 1.0<millipascal> |> box
        | "http://dbpedia.org/datatype/pascal" -> fun v -> float v * 1.0<pascal> |> box

        // speed
        | "http://dbpedia.org/datatype/knot" -> fun v -> float v * 1.0<knot> |> box
        | "http://dbpedia.org/datatype/footPerMinute" -> fun v -> float v * 1.0<foot/minute> |> box
        | "http://dbpedia.org/datatype/footPerSecond" -> fun v -> float v * 1.0<foot/second> |> box
        | "http://dbpedia.org/datatype/milePerHour" -> fun v -> float v * 1.0<mile/hour> |> box
        | "http://dbpedia.org/datatype/kilometrePerSecond" -> (fun (v:string) -> box(float v)), 
                                                              mb.AnnotateType(typeof<float>, [mb.Ratio(mb.SI "kilometre", mb.SI "second")])
        | "http://dbpedia.org/datatype/metrePerSecond" -> fun v -> float v * 1.0<metre/second> |> box
        | "http://dbpedia.org/datatype/kilometrePerHour" -> fun v -> float v * 1.0<kilometre/hour> |> box

        // temperature
        | "http://dbpedia.org/datatype/degreeRankine" -> fun v -> float v * 1.0<degreeRankine> |> box
        | "http://dbpedia.org/datatype/degreeFahrenheit" -> fun v -> float v * 1.0<degreeFahrenheit> |> box
        | "http://dbpedia.org/datatype/kelvin" -> fun v -> float v * 1.0<kelvin> |> box
        | "http://dbpedia.org/datatype/degreeCelsius" -> fun v -> float v * 1.0<degreeCelcius> |> box

        // time
        | "http://dbpedia.org/datatype/day" -> fun v -> float v * 1.0<day> |> box
        | "http://dbpedia.org/datatype/hour" -> fun v -> float v * 1.0<hour> |> box
        | "http://dbpedia.org/datatype/minute" -> fun v -> float v * 1.0<minute> |> box
        | "http://dbpedia.org/datatype/second" -> fun v -> float v * 1.0<second> |> box

        // torque
        | "http://dbpedia.org/datatype/poundFoot" -> fun v -> float v * 1.0<pound foot> |> box
        | "http://dbpedia.org/datatype/newtonCentimetre" -> fun v -> float v * 1.0<newton centimetre> |> box
        | "http://dbpedia.org/datatype/newtonMillimetre" -> fun v -> float v * 1.0<newton millimetre> |> box
        | "http://dbpedia.org/datatype/newtonMetre" -> fun v -> float v * 1.0<newton metre> |> box

        // volume
        | "http://dbpedia.org/datatype/usGallon" -> fun v -> float v * 1.0<usGallon> |> box
        | "http://dbpedia.org/datatype/imperialGallon" -> fun v -> float v * 1.0<imperialGallon> |> box
        | "http://dbpedia.org/datatype/usBarrelOil" -> fun v -> float v * 1.0<usBarrelOil> |> box
        | "http://dbpedia.org/datatype/imperialBarrelOil" -> fun v -> float v * 1.0<imperialBarrelOil> |> box
        | "http://dbpedia.org/datatype/usBarrel" -> fun v -> float v * 1.0<usBarrel> |> box
        | "http://dbpedia.org/datatype/imperialBarrel" -> fun v -> float v * 1.0<imperialBarrel> |> box
        | "http://dbpedia.org/datatype/cubicInch" -> fun v -> float v * 1.0<inch^3> |> box
        | "http://dbpedia.org/datatype/cubicFoot" -> fun v -> float v * 1.0<foot^3> |> box
        | "http://dbpedia.org/datatype/cubicYard" -> fun v -> float v * 1.0<yard^3> |> box
        | "http://dbpedia.org/datatype/cubicMile" -> fun v -> float v * 1.0<mile^3> |> box
        | "http://dbpedia.org/datatype/gigalitre" -> fun v -> float v * 1.0<gigalitre> |> box
        | "http://dbpedia.org/datatype/megalitre" -> fun v -> float v * 1.0<megalitre> |> box
        | "http://dbpedia.org/datatype/kilolitre" -> fun v -> float v * 1.0<kilolitre> |> box
        | "http://dbpedia.org/datatype/hectolitre" -> fun v -> float v * 1.0<hectolitre> |> box
        | "http://dbpedia.org/datatype/litre" -> fun v -> float v * 1.0<litre> |> box
        | "http://dbpedia.org/datatype/decilitre" -> fun v -> float v * 1.0<decilitre> |> box
        | "http://dbpedia.org/datatype/centilitre" -> fun v -> float v * 1.0<centilitre> |> box
        | "http://dbpedia.org/datatype/millilitre" -> fun v -> float v * 1.0<millilitre> |> box
        | "http://dbpedia.org/datatype/microlitre" -> fun v -> float v * 1.0<microlitre> |> box
        | "http://dbpedia.org/datatype/cubicKilometre" -> fun v -> float v * 1.0<kilometre^3> |> box
        | "http://dbpedia.org/datatype/cubicHectometre" -> fun v -> float v * 1.0<hectometre^3> |> box
        | "http://dbpedia.org/datatype/cubicDecametre" -> fun v -> float v * 1.0<decametre^3> |> box
        | "http://dbpedia.org/datatype/cubicCentimetre" -> fun v -> float v * 1.0<centimetre^3> |> box
        | "http://dbpedia.org/datatype/cubicMillimetre" -> fun v -> float v * 1.0<millimetre^3> |> box
        | "http://dbpedia.org/datatype/cubicMetre" -> fun v -> float v * 1.0<metre^3> |> box
        | _ -> failwithf "Unrecognized datatype '%s' (with value '%s')" uri
*)