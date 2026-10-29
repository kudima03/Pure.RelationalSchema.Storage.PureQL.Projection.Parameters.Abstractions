# Changelog

All notable changes to `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `IListParameter`: a list bound to a list parameter (`ListParamInteger` … `ListParamUuid`, read by `in`), the Pure-style counterpart of the PureQL list literals `ListLiteralInteger` … `ListLiteralUuid`. It is an `IParameter` whose `Type` is the list type (`integerList` … `uuidList`, never nullable), with `Items`, the list's values as parameters of the item type, never typed nulls. Its `Value` is informational: a JSON array of the items' cell texts, such as `["paid","shipped"]`.

## [0.1.0-preview.0.1.0] - 2026-10-29

### Added

- `IParameterType`: the PureQL type of a bound query parameter, its name and whether it is the nullable (typed-null) variant.
- `IParameter`: a value bound to a query parameter of `Pure.RelationalSchema.Storage.PureQL.Projection`, its type and its value as invariant cell text; the Pure-style counterpart of the PureQL literals.

[Unreleased]: https://github.com/kudima03/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions/compare/0.1.0-preview.0.1.0...HEAD
[0.1.0-preview.0.1.0]: https://github.com/kudima03/Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions/releases/tag/0.1.0-preview.0.1.0
