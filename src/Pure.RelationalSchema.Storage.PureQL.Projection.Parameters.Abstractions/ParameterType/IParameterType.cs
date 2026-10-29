using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.String;

namespace Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.ParameterType;

/// <summary>
/// The PureQL type of a bound query parameter: the counterpart of the type of a PureQL
/// literal (<c>PureQL.CSharp.Model.Types.IType</c>), for example <c>TypeInteger</c> or
/// <c>TypeIntegerNullable</c>.
/// </summary>
public interface IParameterType
{
    /// <summary>
    /// The PureQL type name: <c>integer</c>, <c>decimal</c>, <c>string</c>,
    /// <c>boolean</c>, <c>date</c>, <c>time</c>, <c>datetime</c> or <c>uuid</c>; for a
    /// list parameter, the list type name, such as <c>stringList</c>.
    /// </summary>
    public IString Name { get; }

    /// <summary>
    /// True for the nullable variant of the type, the type of a typed null such as
    /// <c>LiteralIntegerNullable</c>; false for the type of a literal that holds a
    /// value, such as <c>LiteralInteger</c>, and for a list type.
    /// </summary>
    public IBool Nullable { get; }
}
