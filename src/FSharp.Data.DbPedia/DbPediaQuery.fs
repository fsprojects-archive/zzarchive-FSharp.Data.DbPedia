module DbPediaQuery

open System.Linq
open System.Linq.Expressions
open System.Collections
open System.Collections.Generic
open System.Text
open System.Reflection

//type DbPediaQuery<'T>(provider : DbPediaQueryProvider, expression) =    
//    interface IQueryable with
//        member this.Provider = provider :> IQueryProvider
//        member this.Expression = expression
//        member this.ElementType = typeof<'T>
//        
//    interface IEnumerable with
//        member this.GetEnumerator() = (provider.Execute(expression) :?> IEnumerable).GetEnumerator()
//
//    interface IEnumerable<'T> with
//        member this.GetEnumerator() = (provider.Execute<'T>(expression) :?> IEnumerable<'T>).GetEnumerator()
//
//    interface IQueryable<'T>
//    interface IOrderedQueryable
//    interface IOrderedQueryable<'T>
//
//    override this.ToString() = provider.GetQueryText(expression)

type FirstSomeBuilder() =
    member __.Return(x) = x
    member __.Bind(p, rest) = match p with
                              | Some x -> Some x
                              | None -> rest ()
let firstSome = new FirstSomeBuilder()

[<AutoOpen>]
module Helpers =
    let (|MethodWithName|_|) (s:string) (m:MethodInfo) =  if s = m.Name then Some () else None
    let (|PropertyWithName|_|) (s:string) (m:PropertyInfo) =  if s = m.Name then Some () else None

    let (|MethodCall|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.Call, (:? MethodCallExpression as e) ->  
            Some ((match e.Object with null -> None | obj -> Some obj), e.Method, Seq.toList e.Arguments)
        | _ -> None

    let (|AsType|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.TypeAs, (:? UnaryExpression as e) ->  Some (e.Operand, e.Type)
        | _ -> None

    let (|NewArray|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.NewArrayInit, (:? NewArrayExpression as e) ->  Some (Seq.toList e.Expressions)
        | _ -> None

    let (|PropertyGet|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.MemberAccess, ( :? MemberExpression as e) -> 
            match e.Member with 
            | :? PropertyInfo as p -> 
                    Some ((match e.Expression with null -> None | obj -> Some obj), p)
            | _ -> None
        | _ -> None

    let (|Constant|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.Constant, (:? ConstantExpression as ce) ->  Some (ce.Value, ce.Type)
        | _ -> None

    let (|String|_|) = function | Constant((:? string as s),_) -> Some s | _ -> None
    let (|Int32|_|) = function | Constant((:? int as s),_) -> Some s | _ -> None
    let (|Null|_|) = function Constant(null,_) -> Some () | _ -> None
    let (|Double|_|) = function Constant((:? double as s),_) -> Some s | _ -> None
    let (|Decimal|_|) = function Constant((:? decimal as s),_) -> Some s | _ -> None

    let (|Convert|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.Convert, (:? UnaryExpression as ce) ->  Some (ce.Operand, ce.Type)
        | _ -> None
    
    let (|Var|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.Parameter, (:? ParameterExpression as ce) ->  Some ce
        | _ -> None

    let (|Lambda|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.Lambda, (:? LambdaExpression as ce) ->  Some (Seq.toList ce.Parameters, ce.Body)
        | _ -> None

    let (|LetExpr|_|) (e:Expression) = 
        match e with 
        | MethodCall(Some (Lambda([v],body)), m, [arg]) when m.Name = "Invoke" ->  Some(v,arg,body)
        | _ -> None

    let (|Quote|_|) (e:Expression) = 
        match e.NodeType, e with 
        | ExpressionType.Quote, (:? UnaryExpression as ce) -> Some ce.Operand
        | _ -> None

    let (|Select|_|) (e:Expression) =
        match e with
        | MethodCall(_, MethodWithName "Select", [body; iterVar]) -> Some (body, iterVar)
        | _ -> None

    let rec tryFindInTree p (root : Expression) =
        if p root then 
            Some root 
        else
            match root with
            | MethodCall(_, _, [body; iterVar]) -> 
                firstSome {
                    let! _ = tryFindInTree p body
                    return tryFindInTree p iterVar
                }
            | Quote expr -> tryFindInTree p expr
            | Lambda([parameter],body) ->
                firstSome {
                    let! _ = tryFindInTree p parameter
                    return tryFindInTree p body
                }
            | Var(_) 
            | Constant(_,_) -> None
            | _ -> failwithf "Unrecognized node: %A, predicate: %A" root p

    type Triple = 
        {   Subject : string
            Predicate : string
            Object : string }
        override this.ToString() =
            sprintf "   %s %s %s.\n" this.Subject this.Predicate this.Object

    //    type SparqlFunction = {
    //        
    //    }

    type DbPediaQuery =
        {   SelectFields : string list
            WhereClause : Triple list }
    //        Functions : SparqlFunction list
        override this.ToString() =
            let fields = String.concat "," this.SelectFields
            let triples = String.concat "" (seq {for t in this.WhereClause -> t.ToString()})
            sprintf "SELECT %s WHERE {\n%s}\n" fields triples

type DbPediaQueryableStatics() =
    static member Provider(uri) = { new IQueryProvider with
        member x.CreateQuery(e: Expression): IQueryable = 
            failwithf "CreateQuery, expression %A = %A" e.NodeType e
                                       
        member x.CreateQuery<'T>(e: Expression): IQueryable<'T> = 

            null
                                       
        member x.Execute<'T>(e: Expression): 'T = 
            let optSelect = tryFindInTree (function Select _ -> true | _ -> false) e
            
            let optIterVar = 
                match optSelect with
                | Some (:? MethodCallExpression as selectNode) -> tryFindInTree (function Var _ -> true | _ -> false) selectNode.Arguments.[1] 
                | None -> failwith "Missing select clause"
                | Some _ -> failwith "optSelect is not a MethodCallExpression"

            let iterVar = 
                match optIterVar with
                | Some (:? ParameterExpression as varNode) -> varNode.Name
                | None -> failwith "Missing iteration variable"
                | Some other -> failwithf "optIterVar is not a ParameterExpression, it is a %A" other.NodeType
            
            let sparqlQuery = {SelectFields = ["?" + iterVar]
                               WhereClause = [{Subject="?"+iterVar; Predicate="rdf:type"; Object="<"+uri+">"}]  }
            printfn "SPARQL query:\n%O" sparqlQuery

            failwith "Not finished yet"
                                       
        member x.Execute(expression: Expression): obj = 
            failwith "Not implemented yet"
    }
