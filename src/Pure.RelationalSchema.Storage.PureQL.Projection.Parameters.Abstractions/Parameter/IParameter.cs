using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.ParameterType;

namespace Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.Parameter;

/// <summary>
/// The value bound to a query parameter of
/// <c>Pure.RelationalSchema.Storage.PureQL.Projection</c>: the counterpart of a PureQL
/// literal (<c>PureQL.CSharp.Model.Literals.ILiteral</c>, from <c>LiteralInteger</c> to
/// <c>LiteralUuidNullable</c>), with its value held as the text a stored cell holds.
/// </summary>
public interface IParameter
{
    /// <summary>
    /// The PureQL type of the bound value; a nullable type marks a typed null.
    /// </summary>
    public IParameterType Type { get; }

    /// <summary>
    /// The bound value as invariant cell text, in the format the projection writes and
    /// parses stored cells: an integer such as <c>-42</c>; an exact decimal without an
    /// exponent or trailing zeros in the fraction, such as <c>1.5</c>; the string itself;
    /// <c>True</c> or <c>False</c>; a <c>yyyy-MM-dd</c> date; a <c>HH:mm:ss</c> time; a
    /// <c>yyyy-MM-ddTHH:mm:ss</c> datetime in UTC, without an offset; a lower case
    /// <c>D</c>-format uuid. Times and datetimes carry fractional seconds, up to seven
    /// digits, only when present. A typed null is the empty string.
    /// </summary>
    public IString Value { get; }
}
