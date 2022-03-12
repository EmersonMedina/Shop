using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }
        
        [Display(Name ="País")]
        [MaxLength(50, ErrorMessage ="El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string Name { get; set; }
        //Para decirle a Entity Framework que un país puede estar relacionado con varios estados
        public ICollection<State> States { get; set; }
        [Display(Name = "Departamentos / Estados")]
        public int StatesNumber => States == null? 0 : States.Count;
    }
}
