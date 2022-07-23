using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Ingrese el Código del producto")]
        [Display(Name = "Código del producto")]
        [DataType(DataType.Text)]
        public string Code { get; set; } = null!;
        
        [Display(Name = "Descripción")]
        [DataType(DataType.Text)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Ingrese el Precio")]
        [Display(Name = "Precio unitario")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        [Display(Name = "Imagen del producto")]
        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }
        [Required(ErrorMessage = "Ingrese el total de stock")]
        [Display(Name = "Stock")]
        [DataType(DataType.Currency)]
        public int Stock { get; set; }
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
