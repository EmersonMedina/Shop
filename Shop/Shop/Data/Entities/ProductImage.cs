using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7057/images/noimage.png"
            : $"https://shopasp.blob.core.windows.net/products/{ImageId}";
    }

}
