
using System.Linq.Expressions;
using System;
using FormAdvanced.BuildingBlocks.Application.Configuration.Pagination;

namespace FormAdvanced.BuildingBlocks.Application.Configuration.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Sorts a Queryable by using the SortModel model
        /// </summary>
        /// <typeparam name="T">The type of the Queryable</typeparam>
        /// <param name="source">The queryable to sort</param>
        /// <param name="model">The model by which to sort</param>
        /// <returns>The sorted Queryable</returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, SortModel model)
        {
            var expression = source.Expression;
            var parameter = Expression.Parameter(typeof(T), "x");
            var selector = Expression.PropertyOrField(parameter, model.ColId);
            var method = string.Equals(model.Sort, "desc", StringComparison.OrdinalIgnoreCase) ?
                "OrderByDescending" :
                "OrderBy";
            expression = Expression.Call(typeof(Queryable), method,
                new Type[] { source.ElementType, selector.Type },
                expression, Expression.Quote(Expression.Lambda(selector, parameter)));

            return source.Provider.CreateQuery<T>(expression);
        }

        public static IQueryable<TEntity> FilterBy<TEntity>(this IQueryable<TEntity> source, FilterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Filter) || string.IsNullOrWhiteSpace(model.ColIds))
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression combined = null;
            var colIds = model.ColIds.Split(',');

            foreach (var colId in colIds)
            {
                var property = Expression.PropertyOrField(parameter, colId.Trim());
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var filterExpression = Expression.Call(property, containsMethod, Expression.Constant(model.Filter, typeof(string)));

                combined = combined == null ? filterExpression : Expression.OrElse(combined, filterExpression);
            }

            if (combined == null)
            {
                return source;
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(combined, parameter);
            return source.Where(lambda);
        }
    }
}
