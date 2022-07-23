using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class StoreModel
    {
        [Display(Name = "Identificador")]
        public int StoreId { get; set; }
        [Required(ErrorMessage = "Ingrese el nombre de la sucursal")]
        [Display(Name = "Nombre sucursal")]
        [DataType(DataType.Text)]
        public string Branch { get; set; } = null!;
        [Display(Name = "Dirección")]
        [DataType(DataType.Text)]
        public string? Address { get; set; }
        [Display(Name = "Estatus")]
        public bool? Active { get; set; }
        [Display(Name = "Fecha de alta")]
        [DataType(DataType.Date)]
        public DateTime? DateInserted { get; set; }
        [Display(Name = "Fecha de última modificación")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }
    }
}
