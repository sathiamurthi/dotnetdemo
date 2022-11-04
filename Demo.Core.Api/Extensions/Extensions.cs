using Demo.Core.Domain.Models;
using LinqKit;
using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Demo.Core.Api.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> query, params Sort[] sorts)
        {
            if (sorts == null)
                return query;
            var applicableSorts = sorts.Where(s => s != null);
            if (!applicableSorts.Any())
                return query;
            applicableSorts
                .Select((item, index) => new { Index = index, item.PropertyName, item.Direction })
                .ToList()
                .ForEach(sort =>
                {
                    ParameterExpression parameterExpression = Expression.Parameter(query.ElementType, "entity");
                    var propertyExpression = Expression.Property(parameterExpression, sort.PropertyName);
                    var sortPredicate = Expression.Lambda(propertyExpression, parameterExpression);
                    string methodName = (sort.Index == 0 ? "Order" : "Then") + (sort.Direction == SortOrder.Ascending ? "By" : "ByDescending");
                    MethodCallExpression orderBy = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, propertyExpression.Type }, query.Expression, Expression.Quote(sortPredicate));
                    query = query.Provider.CreateQuery<TEntity>(orderBy);
                });

            return query;
        }
        public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> query, params Filter[] filter)
        {
            if (filter == null)
                return query;
            var applicableSorts = filter.Where(s => s != null);
            if (!applicableSorts.Any())
                return query;

            Type t = typeof(TEntity);
            var andCriteria = new List<Predicate<TEntity>>();
            Expression<Func<TEntity, bool>> predicate = null;
            Expression<Func<TEntity, bool>> predicate1 = PredicateBuilder.New<TEntity>();

            foreach (var name in filter)
            {

                var type = t.GetProperty(name.PropertyName);
                predicate1 = predicate1.Or(c => type.GetValue(c).ToString().ToLower().Contains(name.PropertyValue));


                //var type = t.GetProperty(name.PropertyName);
                // andCriteria.Add(c => type.GetValue(c).ToString().ToLower().Contains(name.PropertyValue));
                //predicate = c => andCriteria.All(pred => pred(c));
            }

            query = query.Where(predicate1);

            ////Expression whereExpression = null;
            ////Expression<Func<Error, bool>> lambda = null;
            //ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "item");


            ////foreach (var name in filter)
            ////{
            ////    Expression whereProperty = Expression.Property(parameter, name.PropertyName);
            ////    var toLower = Expression.Call(whereProperty, "ToLower", null);
            ////    // Expression constant = Expression.Constant(filter.PropertyValue);
            ////    //Expression condition = Expression.Equal(whereProperty, constant);
            ////    var stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ////    var containsCall = Expression.Call(toLower, stringContainsMethod, Expression.Constant(name.PropertyValue, typeof(string)));
            ////    //lambda = Expression.Lambda<Func<Error, bool>>(equalExpr, error);
            ////   // lambda = (lambda == null)? Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter) : Expression.Lambda<Func<TEntity, bool>>(lambda, parameter);
            ////}

            //applicableSorts
            //    .Select((item, index) => new { item.PropertyName, item.PropertyValue })
            //    .ToList()
            //    .ForEach(filter =>
            //    {


            //       // ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "item");
            //        Expression whereProperty = Expression.Property(parameter, filter.PropertyName);
            //        var toLower = Expression.Call(whereProperty, "ToLower", null);
            //       // Expression constant = Expression.Constant(filter.PropertyValue);
            //        //Expression condition = Expression.Equal(whereProperty, constant);
            //        var stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            //        var containsCall = Expression.Call(toLower, stringContainsMethod, Expression.Constant(filter.PropertyValue, typeof(string)));
            //        // var expbrandCode = Expression.Call(typeof(string), "Equals", null, me, constant, Expression.Constant(StringComparison.CurrentCultureIgnoreCase));
            //        //Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
            //        Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
            //        query = query.Where(lambda);
            //    });

            return query;
        }

        public static (IQueryable<TEntity> employees, Pagination pagination) ApplyPagination<TEntity>(IQueryable<TEntity> employees, Pagination pagination)
        {
            int pageSize = pagination.recordsPerPage == 0 ? 5 : pagination.recordsPerPage;
            Pagination page = new Pagination()
            {
                recordsPerPage = pageSize,
                pageIndex = pagination.pageIndex == 0? 1:pagination.pageIndex,
                totalRecords = employees.Count()
            };
            return (employees.Skip(pagination.pageIndex * pageSize).Take(pageSize), page);
        }
    }

    public class Sort
    {
        public string Index { get; set; }
        public string PropertyName { get; set; }
        public SortOrder Direction { get; set; }

    }
    public class SortQuery : Employee
    {
        private new int Id { get; set; }
    }

    public static class Helper
    {
        public static Sort[] GetSorting(object source)
        {
            if (source == null)
            {
                return new Sort[] { new Sort() { PropertyName = "Id", Index = "0", Direction = SortOrder.Ascending } };
            }
            Type t = source.GetType();
            Sort[] sortlist = new Sort[t.GetProperties().Length];
            int i = 0;
            //foreach (var propInfo in t.GetProperties())
            {
               // if (propInfo.GetValue(source) == null) continue;
                //if (propInfo.PropertyType == typeof(string))
                {

                    sortlist[i] = new Sort();
                    PropertyInfo propertyName = sortlist[i].GetType().GetProperty("PropertyName");
                    PropertyInfo direction = sortlist[i].GetType().GetProperty("Direction");
                    PropertyInfo propertyIndex = sortlist[i].GetType().GetProperty("Index");

                    //PropertyInfo pi = t.GetProperty("propertyName");

                    object sortByField = t.GetProperty("sortBy").GetValue(source, null);

                    object sortDirection = t.GetProperty("sortOrder").GetValue(source, null);

                    propertyIndex.SetValue(sortlist[i], (i).ToString(), null);
                    propertyName.SetValue(sortlist[i], sortByField, null);
                    var propVal = sortDirection != null ? sortDirection : string.Empty;
                    direction.SetValue(sortlist[i], GetValue(propVal.ToString()), null);

                }
            }
            var count = sortlist.Where(c => c != null).Count();
            if (count == 0)
            {
                sortlist[0] = new Sort() { PropertyName = "Id", Index = "0", Direction = SortOrder.Ascending };
            }

            return sortlist.Where(c => c != null).ToArray();
        }

        internal static Filter[]? GetFilter(object filter)
        {
            if (filter == null)
            {
                return null;
            }
            Type t = filter.GetType();
            PropertyInfo[] propertyList = t.GetProperties().Where(e => e.GetValue(filter) != null).ToArray();
            Filter[] filterList = new Filter[t.GetProperties().Length];
            int i = 0;
            foreach (var propInfo in propertyList)
            {
                if (propInfo.GetValue(filter) == null) continue;
                if (propInfo.PropertyType == typeof(string))
                {

                    filterList[i] = new Filter();
                    PropertyInfo propertyName = filterList[i].GetType().GetProperty("PropertyName");
                    PropertyInfo propertyValue = filterList[i].GetType().GetProperty("PropertyValue");

                    object propVal = propInfo.GetValue(filter, null);
                    propertyName.SetValue(filterList[i], propInfo.Name, null);
                    propertyValue.SetValue(filterList[i], propVal.ToString(), null);
                    i++;
                }
            }
            var count = filterList.Where(c => c != null).Count();
            if (count == 0)
            {
                return null;
            }

            return filterList.Where(c => c != null).ToArray();
        }

        public static string GetBaseUrl(HttpRequest request)
        {
            // SSL offloading
            var scheme = request.Host.Host.Contains("localhost") ? request.Scheme : "https";
            return $"{scheme}://{request.Host}{request.PathBase}";
        }
        private static SortOrder? GetValue(string? propVal)
        {
            if (string.IsNullOrEmpty(propVal))
            {
                return SortOrder.Ascending;
            }
            if (propVal.ToLower().Equals("asc") || propVal.ToLower().Equals("ascending"))
            {
                return SortOrder.Ascending;
            }
            if (propVal.ToLower().Equals("desc") || propVal.ToLower().Equals("descending"))
            {
                return SortOrder.Descending;
            }
            return SortOrder.Ascending;
        }
    }

}
