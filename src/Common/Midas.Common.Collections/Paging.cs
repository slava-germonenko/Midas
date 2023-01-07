namespace Midas.Common.Collections;

public record Paging
{
    public const int DefaultOffset = 0;

    public const int DefaultLimit = 50;

    public int Offset { get; set; } = DefaultOffset;

    public int Limit { get; set; } = DefaultLimit;
}