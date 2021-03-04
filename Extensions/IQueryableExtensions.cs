using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using vega.Core.Models;

namespace vega.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject qryObject, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (String.IsNullOrWhiteSpace(qryObject.SortBy) || !columnsMap.ContainsKey(qryObject.SortBy))
                return query;

            if (qryObject.IsSortAscending)
                return query = query.OrderBy(columnsMap[qryObject.SortBy]);
            else
                return query = query.OrderByDescending(columnsMap[qryObject.SortBy]);
        }
    }
}