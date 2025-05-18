using Data.Filters;
using Data.Models;
using Data.Repositories.Interfaces;
using Data.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class UserExtension
    {

        public static IQueryable<User> Filter(this IQueryable<User> list, UserFilter userFilter)
        {

            if (!string.IsNullOrEmpty(userFilter.FirstName))
            {
                list = list.Where(u => u.FirstName == userFilter.FirstName);
            }

            if (!string.IsNullOrEmpty(userFilter.LastName))
            {
                list = list.Where(u => u.LastName == userFilter.LastName);
            }

            return list;
        }

        public static IQueryable<User> Sort(this IQueryable<User> list, SortParameters sortParameters)
        {
            if (sortParameters.SortDirection == SortDirection.Descending)
                return list.OrderByDescending(GetKeySelector(sortParameters.OrderBy));

            return list.OrderBy(GetKeySelector(sortParameters.OrderBy));
        }

        private static Expression<Func<User, object>> GetKeySelector(string? orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
                return x => x.Email;

            return orderBy switch
            {
                nameof(User.FirstName) or "Имя" => x => x.FirstName,
                nameof(User.LastName) or "Фамилия" => x => x.LastName,
                nameof(User.Created) or "Время добавления" => x => x.LastName,
                _ => x => x.Email
            };
        }


    }
}
