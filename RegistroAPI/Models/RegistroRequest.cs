namespace RegistroAPI.Models
{
    public class RegistroRequest
    {
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        // No se necesita ConfirmarContrasena aquí, ya que la validación se haría en el frontend
    }
}