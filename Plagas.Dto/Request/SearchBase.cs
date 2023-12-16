using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagas.Dto.Request
{
    public class SearchBase
    {
        public int Page { get; set; } = 1;
        public int Rows { get; set; } = 10;
    }
}
