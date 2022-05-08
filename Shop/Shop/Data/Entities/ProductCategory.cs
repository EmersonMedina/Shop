using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Category Category { get; set; }
    }

}
