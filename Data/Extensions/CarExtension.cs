using Data.Filters;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class CarExtension
    {
        public static IQueryable<Car> Filter(this IQueryable<Car> list, CarFilter carFilter)
        {

            if (!string.IsNullOrEmpty(carFilter.Brand))
            {
                list = list.Where(c => c.Brand == carFilter.Brand);
            }

            if (carFilter.Year != 0)
            {
                list = list.Where(c => c.Year == carFilter.Year);
            }

            return list;
        }
    }
}
