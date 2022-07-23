using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class ClientModel
    {
        [Display(Name = "Identificador")]
        public int? ClientId { get; set; }
        [Required(ErrorMessage = "Ingrese su nombre")]
        [Display(Name = "Nombre")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Ingrese su apellido paterno")]
        [Display(Name = "Apellido paterno")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Favor de ingresar su correo electrónico")]
        [Display(Name = "Correo electrónico")]
        [EmailAddress(ErrorMessage = "Favor de ingresar un correo válido")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Favor ingrese su contraseña")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Display(Name = "Estatus")]
        public bool? Active { get; set; }
            [Display(Name = "Fecha de inicio")]
            [DataType(DataType.Date)]
        public DateTime? DateInserted { get; set; }
        [Display(Name = "Fecha de última modificación")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }
    }
}