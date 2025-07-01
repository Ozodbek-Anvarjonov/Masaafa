using Masaafa.Domain.Common.Pagination;
using System.Linq.Expressions;

namespace Masaafa.Persistence.Extensions;

public static class SortingExtension
{
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, Filter? sorting = default)
    {
        if (sorting is null || sorting.OrderBy is null || sorting.OrderType is null)
            return source;

        var expression = source.Expression;
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        MemberExpression selector;
        try
        {
            selector = Expression.PropertyOrField(parameter, sorting.OrderType!);
        }
        catch
        {
            throw new InvalidOperationException("Specified property is not found");
        }

        var method = string.Equals(sorting?.OrderBy ?? "asc", "desc", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

        expression = Expression.Call(typeof(Queryable), method,
            new Type[] { source.ElementType, selector.Type },
            expression, Expression.Quote(Expression.Lambda(selector, parameter)));

        return source.Provider.CreateQuery<TEntity>(expression);
    }
}