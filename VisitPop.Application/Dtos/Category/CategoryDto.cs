using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitPop.Application.Dtos.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // add-on property marker - Do Not Delete This Comment
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
