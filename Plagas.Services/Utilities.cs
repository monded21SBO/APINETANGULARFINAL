using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagas.Services
{
    public static class Utilities
    {
        public static int GetTotalPages(int total, int rows)
        {
            if (total == 0)
                return 0;

            var totalPages = total / rows;
            if (total % rows > 0)
            {
                totalPages++;
            }

            return totalPages;
        }
    }
}
