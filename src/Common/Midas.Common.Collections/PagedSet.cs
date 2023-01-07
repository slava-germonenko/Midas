namespace Midas.Common.Collections;

public record PagedSet<TItem>
{
    public int Total { get; set; }

    public required ICollection<TItem> Items { get; set; }
}