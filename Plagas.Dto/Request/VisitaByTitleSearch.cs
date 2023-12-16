using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagas.Dto.Request
{
    public class VisitaByTitleSearch : SearchBase
    {
        public string? Nombre { get; set; }
    }
}
