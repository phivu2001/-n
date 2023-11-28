using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ResponseModels
    {
        public int? ErrorNumber { get; set; }
        public dynamic Message { get; set; }
        public dynamic Data { get; set; }
        public bool Success { get; set; }
        public long? Id { get; set; } = 0;
        public int Status { get; set; } = 200;
    }
}
