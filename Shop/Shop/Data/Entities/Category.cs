﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Categoría")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
