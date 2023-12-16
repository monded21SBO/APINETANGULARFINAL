using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagas.Dto.Request
{
    public class VisitaByDateSearch : SearchBase
    {
        public string DateStart { get; set; } = default!;
        public string DateEnd { get; set; } = default!;

    }
}
