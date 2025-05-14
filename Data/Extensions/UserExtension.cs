using Data.Filters;
using Data.Models;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
