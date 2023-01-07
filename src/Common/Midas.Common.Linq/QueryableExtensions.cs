using Microsoft.EntityFrameworkCore;
using Midas.Common.Collections;

namespace Midas.Common.Linq;

public static class QueryableExtensions
{
    public static async Task<PagedSet<TEntity>> ToPagedSetAsync<TEntity>(
        this IQueryable<TEntity> queryable,
        Paging paging,
        CancellationToken cancellationToken = new()
    )
    {
        var count = await queryable.CountAsync();

        var pagedQuery = queryable.Skip(paging.Offset).Take(paging.Limit);
        var items = new List<TEntity>(paging.Limit);

        await foreach (var element in pagedQuery.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            items.Add(element);
        }

        return new()
        {
            Total = count,
            Items = items,
        };
    }
}