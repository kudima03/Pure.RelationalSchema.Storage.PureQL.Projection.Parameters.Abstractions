# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes             # check code style (CI enforces this)
dotnet format                                 # auto-fix code style
dotnet pack --configuration Release -p:Version=<version> --output .
```

CI additionally passes `-p:AssemblyVersion` (pinned to the major) and `-p:FileVersion`; see `.github/workflows/publish-nuget.yml`.

There are no test or benchmark projects. The CI pipeline (`build-and-check.yml`) only builds and checks formatting. Do not run csharpier: its default width of 100 conflicts with the 90-column limit. Its CI step is commented out on purpose, and `dotnet format` is the authoritative style check.

## Architecture

This is an **interfaces-only NuGet library**: no implementations, no tests. Every file defines exactly one interface. It describes a value bound to a query parameter of `Pure.RelationalSchema.Storage.PureQL.Projection`: the Pure-style counterpart of a PureQL literal (`PureQL.CSharp.Model.Literals.ILiteral` and its 16 records, `LiteralInteger` … `LiteralUuidNullable`, and the eight list literals of `PureQL.CSharp.Model.Lists`, `ListLiteralInteger` … `ListLiteralUuid`), built from Pure primitives only, so that a parameter can be stored or sent as data without depending on the PureQL model.

**Interfaces:** one folder and namespace per concept, like `Pure.RelationalSchema.Abstractions` (`.Schema`, `.Table`, `.Column`):

- `ParameterType/IParameterType`: `IString Name` (`integer`, `decimal`, `string`, `boolean`, `date`, `time`, `datetime`, `uuid`, or a list type name such as `stringList`), `IBool Nullable` (true for a typed null, false for a list type). The counterpart of `PureQL.CSharp.Model.Types.IType`.
- `Parameter/IParameter`: `IParameterType Type`, `IString Value`.
- `ListParameter/IListParameter`: an `IParameter` with `IEnumerable<IParameter> Items`, the counterpart of a list literal, bound to a list parameter (`ListParamInteger` … `ListParamUuid`) that only `in` reads. `Type.Name` is the list type name of `PureQL.CSharp.Model.Types` (`integerList`, `decimalList`, `stringList`, `booleanList`, `dateList`, `timeList`, `datetimeList`, `uuidList`) and `Type.Nullable` is false: the PureQL specification says a list type is never nullable and a list holds no `null`, so `Items` are parameters of the item type that hold a value, never typed nulls. A list may be empty and may repeat a value (the specification forbids repeats in a list literal, not in a list parameter's value).

`Value` is invariant cell text, in the format the projection writes and parses stored cells (`CellText.cs` and `StoredColumn.cs` in that repo): `True`/`False`, invariant integers, exact decimals without exponent or trailing zeros in the fraction, `yyyy-MM-dd` dates, `HH:mm:ss` times, `yyyy-MM-ddTHH:mm:ss` datetimes in UTC without an offset, lower case `D`-format uuids; fractional seconds only when present. A typed null is the empty string. If the projection changes that format, update the XML docs and the README here.

A list parameter's `Value` is informational, since the projection reads `Items`: a JSON array of the items' `Value` texts, each as a JSON string, in order (`["paid","shipped"]`, `["1","2.5"]`, `[]` for an empty list).

Converting a parameter to a literal, parsing and validation belong to the implementations in `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters`. Do not add records here, and do not reference `PureQL.CSharp.Model` or the projection: in XML docs, name their types with `<c>`, not `<see cref>`.

**Dependencies:** only `Pure.Primitives.Abstractions` (4.3.0, the version `Pure.Primitives` 3.7.0 depends on). Keep its version aligned with what the projection resolves, and do not add references to packages that come transitively.

**Documentation:** `GenerateDocumentationFile` is on, so a public type or member without an XML doc comment fails the build (CS1591 under `-warnaserror`). The XML docs ship in the package.

**Multi-targeting:** net8.0, net9.0, net10.0. All interfaces must remain AOT-compatible (`IsAotCompatible=true`).

**Package validation:** `EnablePackageValidation = true`. `PackageValidationBaselineVersion` is the last published version (set after each release); breaking API changes against it fail the build.

**Publishing:** triggered by pushing a semver tag matching `*.*.*`. The tag name becomes the `PackageVersion`. Packages are published to both GitHub Packages and NuGet.org.

## Code Style

Enforced via `.editorconfig` and `dotnet format --verify-no-changes` in CI:

- No `var`: always use explicit types
- No expression-bodied methods or constructors. Properties, indexers, accessors and lambdas may be expression-bodied
- File-scoped namespace declarations; namespaces match folders
- `using` directives outside the namespace, System directives sorted first
- Max line length: 90 characters. The namespace declarations and the `using` directives of this package's own namespaces are longer and cannot be wrapped; that is the only exception
- Braces on new lines (`csharp_new_line_before_open_brace = all`)
- XML doc comments on every public member

## Commit Messages

Conventional Commits (`feat: …`, `fix: …`, `chore: …`, `docs: …`, `ci: …`).
