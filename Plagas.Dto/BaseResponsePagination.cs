using plagas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagas.Dto
{
    public class BaseResponsePagination<T> : BaseResponse
    {
        public ICollection<T>? Data { get; set; }
        public int TotalPages { get; set; }

    }
}
