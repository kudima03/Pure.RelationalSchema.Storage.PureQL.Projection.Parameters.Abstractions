# Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions

Query parameter abstractions for `Pure.RelationalSchema.Storage.PureQL.Projection` — Pure-style counterparts of the PureQL literals and list literals that bind a query parameter.

[![.NET build & check](https://github.com/kudima03/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions/actions/workflows/build-and-check.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions/actions/workflows/build-and-check.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions)](https://www.nuget.org/packages/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

A PureQL query can read named parameters. `Pure.RelationalSchema.Storage.PureQL.Projection` binds them by name to PureQL literals: `PureQL.CSharp.Model.Literals.ILiteral` and its 16 records, from `LiteralInteger(long)` to `LiteralUuidNullable()`, one with a value and one typed null for each of the eight PureQL types. A list parameter (`ListParamInteger` to `ListParamUuid`), which only `in` reads, is bound to a list literal: `ListLiteralInteger(IEnumerable<long>)` to `ListLiteralUuid(IEnumerable<Guid>)` in `PureQL.CSharp.Model.Lists`, one for each type.

This package holds the Pure-style counterparts of those literals. A parameter is its PureQL type and its value as the text a stored cell would hold, built from `Pure.Primitives.Abstractions` only. It does not depend on `PureQL.CSharp.Model`, so a parameter can be stored or sent as data (for example by an AOT-compatible domain layer) and turned into a literal when the query is run. A list parameter is its PureQL list type and its items, each a parameter of the item type. The implementations live in `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters`.

## Installation

```
dotnet add package Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions
```

## Public API

| Name | Namespace | Description |
|------|-----------|-------------|
| `IParameterType` | `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.ParameterType` | `IString Name`, the PureQL type name, and `IBool Nullable`, true for the typed-null variant: the counterpart of `PureQL.CSharp.Model.Types.IType` |
| `IParameter` | `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.Parameter` | `IParameterType Type` and `IString Value`, the bound value as invariant cell text: the counterpart of `ILiteral` |
| `IListParameter` | `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.ListParameter` | An `IParameter` with `IEnumerable<IParameter> Items`, the items of the list as parameters of the item type: the counterpart of the list literals `ListLiteralInteger` … `ListLiteralUuid` |

`Name` is one of `integer`, `decimal`, `string`, `boolean`, `date`, `time`, `datetime` and `uuid`. `Nullable` is true only for a typed null, such as `LiteralIntegerNullable`: the PureQL model has no nullable literal that holds a value. A list parameter's `Name` is one of the list type names below, and its `Nullable` is always false.

### The value text

`Value` is in the format the projection writes result cells and parses stored cells (`CellText` and `StoredColumn` in that repo). It is invariant, so it does not depend on the current culture:

| Type | Text | Literal |
|------|------|---------|
| `integer` | `-42` | `LiteralInteger(-42)` |
| `decimal` | `1.5`: exact, no exponent, no trailing zeros in the fraction | `LiteralDecimal(1.50m)` |
| `string` | the string itself | `LiteralString("Ada")` |
| `boolean` | `True` or `False` | `LiteralBoolean(true)` |
| `date` | `2026-10-29` (`yyyy-MM-dd`) | `LiteralDate(new DateOnly(2026, 10, 29))` |
| `time` | `08:30:00`, `08:30:00.25` (`HH:mm:ss`, fractional seconds only when present) | `LiteralTime(new TimeOnly(8, 30))` |
| `datetime` | `2026-10-29T08:30:00` (`yyyy-MM-ddTHH:mm:ss` in UTC, no offset, fractional seconds only when present) | `LiteralDatetime(...)` |
| `uuid` | `3f2504e0-4f89-11d3-9a0c-0305e82c3301` (lower case, `D` format) | `LiteralUuid(...)` |

A typed null is the empty string, the storage layer's NULL. For a `string` parameter, `Nullable` tells a typed null from the empty string.

### List parameters

An `IListParameter` is the counterpart of a list literal. Its `Type` is the PureQL list type, named as in `PureQL.CSharp.Model.Types` (`TypeIntegerList` … `TypeUuidList`), and its `Items` are parameters of the item type:

| List type | `Nullable` | Items | `Value` | List literal |
|-----------|------------|-------|---------|--------------|
| `integerList` | `False` | `integer` | `["1","-42"]` | `ListLiteralInteger([1, -42])` |
| `decimalList` | `False` | `decimal` | `["1","2.5"]` | `ListLiteralDecimal([1m, 2.50m])` |
| `stringList` | `False` | `string` | `["paid","shipped"]` | `ListLiteralString(["paid", "shipped"])` |
| `booleanList` | `False` | `boolean` | `["True","False"]` | `ListLiteralBoolean([true, false])` |
| `dateList` | `False` | `date` | `["2026-10-29"]` | `ListLiteralDate(...)` |
| `timeList` | `False` | `time` | `["08:30:00","08:30:00.25"]` | `ListLiteralTime(...)` |
| `datetimeList` | `False` | `datetime` | `["2026-10-29T08:30:00"]` | `ListLiteralDatetime(...)` |
| `uuidList` | `False` | `uuid` | `["3f2504e0-4f89-11d3-9a0c-0305e82c3301"]` | `ListLiteralUuid(...)` |

- **Items** are the list's values in order, each an ordinary `IParameter` of the item type with its value as cell text. A PureQL list holds no `null` and a list type is never nullable, so an item is never a typed null. A list may be empty and may repeat a value.
- **`Value`** is informational, since the projection reads `Items`: a JSON array of the items' `Value` texts, each as a JSON string, in order, and `[]` for an empty list.

## Usage

The package ships no implementations. Minimal records over `Pure.Primitives`:

```csharp
using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.ListParameter;
using Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.Parameter;
using Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.ParameterType;

public sealed record ParameterType : IParameterType
{
    public ParameterType(IString name, IBool nullable)
    {
        Name = name;
        Nullable = nullable;
    }

    public IString Name { get; }

    public IBool Nullable { get; }
}

public sealed record Parameter : IParameter
{
    public Parameter(IParameterType type, IString value)
    {
        Type = type;
        Value = value;
    }

    public IParameterType Type { get; }

    public IString Value { get; }
}

public sealed record ListParameter : IListParameter
{
    public ListParameter(IParameterType type, IEnumerable<IParameter> items, IString value)
    {
        Type = type;
        Items = items;
        Value = value;
    }

    public IParameterType Type { get; }

    public IEnumerable<IParameter> Items { get; }

    public IString Value { get; }
}
```

A `decimal` parameter of `9.99`, a typed-null `date` parameter, and a `stringList` of `paid` and `shipped`:

```csharp
using Pure.Primitives.Bool;
using Pure.Primitives.String;
using String = Pure.Primitives.String.String;

IParameter price = new Parameter(
    new ParameterType(new String("decimal"), new False()),
    new String("9.99")
);

IParameter shippedBefore = new Parameter(
    new ParameterType(new String("date"), new True()),
    new EmptyString()
);

IParameterType status = new ParameterType(new String("string"), new False());

IListParameter statuses = new ListParameter(
    new ParameterType(new String("stringList"), new False()),
    [new Parameter(status, new String("paid")), new Parameter(status, new String("shipped"))],
    new String("""["paid","shipped"]""")
);
```

Build the text yourself, or from a formatted .NET value with `new String(text)`. The `Pure.Primitives` constructors that format an `IDate`, `ITime` or `IDateTime` write another format (`10/29/2026`, for example), which is not cell text.

When the query runs, each parameter becomes the PureQL literal its type names, a list parameter the list literal of its items, and the literals bind the query's parameters by name:

```csharp
using System.Globalization;
using PureQL.CSharp.Model.Lists;
using PureQL.CSharp.Model.Literals;

static ILiteral Literal(IParameter parameter)
{
    string text = parameter.Value.TextValue;
    CultureInfo invariant = CultureInfo.InvariantCulture;

    return (parameter.Type.Name.TextValue, parameter.Type.Nullable.BoolValue) switch
    {
        ("integer", false) => new LiteralInteger(long.Parse(text, invariant)),
        ("decimal", false) => new LiteralDecimal(decimal.Parse(text, invariant)),
        ("date", false) => new LiteralDate(DateOnly.Parse(text, invariant)),
        ("date", true) => new LiteralDateNullable(),
        // ... the other types
        _ => throw new ArgumentException($"{parameter.Type.Name.TextValue} is unknown."),
    };
}

static ILiteral ListLiteral(IListParameter parameter)
{
    IEnumerable<string> texts = parameter.Items.Select(item => item.Value.TextValue);
    CultureInfo invariant = CultureInfo.InvariantCulture;

    return parameter.Type.Name.TextValue switch
    {
        "integerList" => new ListLiteralInteger(
            [.. texts.Select(text => long.Parse(text, invariant))]
        ),
        "stringList" => new ListLiteralString([.. texts]),
        // ... the other list types
        _ => throw new ArgumentException($"{parameter.Type.Name.TextValue} is unknown."),
    };
}

IReadOnlyDictionary<string, ILiteral> literals = new Dictionary<string, ILiteral>
{
    ["price"] = Literal(price),
    ["shippedBefore"] = Literal(shippedBefore),
    ["statuses"] = ListLiteral(statuses),
};

PureQLProjection projection = new PureQLProjection(datasets, query, literals);
```

## Design Principles

- **Interfaces-only** — no implementations, no tests; every file defines exactly one interface
- **Model-agnostic** — depends on `Pure.Primitives.Abstractions` only, never on the PureQL model or the projection
- **AOT-compatible** — `IsAotCompatible=true`
- **Documented** — XML doc comments on every public member, shipped in the package

## Dependencies

- [`Pure.Primitives.Abstractions`](https://github.com/kudima03/Pure.Primitives.Abstractions/tree/4.3.0) — `IString` and `IBool`

## Target Frameworks

- .NET 8
- .NET 9
- .NET 10

## Related packages

- `Pure.RelationalSchema.Storage.PureQL.Projection` — runs PureQL queries over stored relational data and binds their parameters to PureQL literals
- `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters` — the implementations of these interfaces
- `PureQL.CSharp.Model` — the PureQL literals and types these interfaces mirror
