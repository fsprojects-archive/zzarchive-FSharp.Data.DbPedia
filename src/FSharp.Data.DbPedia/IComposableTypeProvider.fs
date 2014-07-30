namespace FSharp.ProvidedTypes.Combinators

open Microsoft.FSharp.Core.CompilerServices

type IComposableTypeProvider =
    inherit ITypeProvider

    /// Provides a type if the string is recognized by the type provider,
    /// otherwise returns None. The input string is intended to uniquely identify
    /// a resource/entity, such as a URL.
    abstract member GetTypeById : string -> System.Type option