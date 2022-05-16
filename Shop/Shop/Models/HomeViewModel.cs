using Shop.Common;
using Shop.Data.Entities;
using System.Collections.Generic;

namespace Shop.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Product> Products { get; set; }

        public ICollection<Category> Categories { get; set; }

        public float Quantity { get; set; }
    }


}
