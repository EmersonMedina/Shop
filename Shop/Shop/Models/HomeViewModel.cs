using Shop.Data.Entities;
using System.Collections.Generic;

namespace Shop.Models
{
    public class HomeViewModel
    {
        public ICollection<Product> Products { get; set; }

        public float Quantity { get; set; }
    }


}
