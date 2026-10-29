using Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.Parameter;

namespace Pure.RelationalSchema.Storage.PureQL.Projection.Parameters.Abstractions.ListParameter;

/// <summary>
/// The list bound to a list parameter of
/// <c>Pure.RelationalSchema.Storage.PureQL.Projection</c>, a parameter that only
/// <c>in</c> reads (<c>PureQL.CSharp.Model.Lists.ListParamInteger</c> to
/// <c>ListParamUuid</c>): the counterpart of a PureQL list literal
/// (<c>PureQL.CSharp.Model.Lists.ListLiteralInteger</c> to <c>ListLiteralUuid</c>),
/// with its items held as parameters of the item type.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="IParameter.Type"/> is the PureQL list type
/// (<c>PureQL.CSharp.Model.Types.TypeIntegerList</c> to <c>TypeUuidList</c>): its name
/// is <c>integerList</c>, <c>decimalList</c>, <c>stringList</c>, <c>booleanList</c>,
/// <c>dateList</c>, <c>timeList</c>, <c>datetimeList</c> or <c>uuidList</c>, and it is
/// never nullable, since a PureQL list type has no typed null.
/// </para>
/// <para>
/// <see cref="IParameter.Value"/> is informational: the projection reads
/// <see cref="Items"/>. It is a JSON array of the items' values, each the item's
/// invariant cell text as a JSON string, in the order of the items, such as
/// <c>["paid","shipped"]</c> or <c>["1","2.5"]</c>, and <c>[]</c> for an empty list.
/// </para>
/// </remarks>
public interface IListParameter : IParameter
{
    /// <summary>
    /// The items of the list, in order: parameters of the item type that hold a value,
    /// such as <c>string</c> parameters for a <c>stringList</c>, each with its value as
    /// invariant cell text. An item is never a typed null, since a PureQL list holds no
    /// <c>null</c>. The list may be empty and may repeat a value.
    /// </summary>
    public IEnumerable<IParameter> Items { get; }
}
