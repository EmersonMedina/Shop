using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Departamento / Estado")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }
        //Para decirle a E.F y relacionar que un estado pertenece a un país
        public Country Country { get; set; }
        public ICollection<City> Cities { get; set; }

        [Display(Name = "Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count; 
    }
}
