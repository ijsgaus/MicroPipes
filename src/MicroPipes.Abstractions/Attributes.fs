namespace MicroPipes
open System
open NuGet.Versioning

[<AttributeUsage(AttributeTargets.All)>]
[<AllowNullLiteral>]
type NameInSchemaAttribute(name: string) =
    inherit Attribute()
    do
        if isNull name then raise (ArgumentNullException "name")
    member __.Name = name

[<AttributeUsage(AttributeTargets.All)>]
[<AllowNullLiteral>]
type VersionAttribute(from: string, last: string) =
    inherit Attribute()
    do
        if isNull from then raise (ArgumentNullException "from")
    let fromVersion = SemanticVersion.Parse(from)
    let toVersion = if isNull last then null else SemanticVersion.Parse(last)
    new(from) = VersionAttribute(from, null)
    member __.FromVersion = fromVersion
    member __.ToVersion = toVersion

[<AttributeUsage(AttributeTargets.Class)>]
[<AllowNullLiteral>]
type OneOfRootAttribute() =
    inherit Attribute()

[<AttributeUsage(AttributeTargets.Class)>]
[<AllowNullLiteral>]
type OneOfNameAttribute(name : string) =
    inherit Attribute()
    do
        if isNull name then raise (ArgumentNullException "name")
    member __.Name = name
