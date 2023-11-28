using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Category
{
    [Table("Category")]
    public class CategoryEntities : BaseEntities
    {
        public string Name { get; set; }
    }
}
