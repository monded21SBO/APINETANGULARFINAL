using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagas.Dto.Response
{
    public class ReportDtoResponse
    {
        public string TrampasName { get; set; } = default!;
        public double Total { get; set; }
    }
}
