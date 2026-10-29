# Changelog

All notable changes to `Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `IParameterType`: the PureQL type of a bound query parameter, its name and whether it is the nullable (typed-null) variant.
- `IParameter`: a value bound to a query parameter of `Pure.RelationalSchema.Storage.PureQL.Projection`, its type and its value as invariant cell text; the Pure-style counterpart of the PureQL literals.
