using System.Linq.Expressions;

namespace AppName.DataCenter.Server.Extensions
{
    internal static class IQueryableExtension
    {
        public static IQueryable<T> WhereIF<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (!condition)
            {
                return query;
            }
            return query.Where(predicate);
        }
    }
}