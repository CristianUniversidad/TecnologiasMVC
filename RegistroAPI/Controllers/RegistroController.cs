using Microsoft.AspNetCore.Mvc;
using RegistroAPI.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "registros.csv");

        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Crear el archivo si no existe y escribir el encabezado
                if (!System.IO.File.Exists(_filePath))
                {
                    var header = "FechaRegistro,NombreCompleto,CorreoElectronico,NombreUsuario,Contrasena" + Environment.NewLine;
                    await System.IO.File.AppendAllTextAsync(_filePath, header, Encoding.UTF8);
                }

                // Formatear los datos para CSV
                var registroLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{EscapeCsvField(request.NombreCompleto)},{EscapeCsvField(request.CorreoElectronico)},{EscapeCsvField(request.NombreUsuario)},{EscapeCsvField(request.Contrasena)}" + Environment.NewLine;

                // Escribir los datos en el archivo
                await System.IO.File.AppendAllTextAsync(_filePath, registroLinea, Encoding.UTF8);

                return Ok(new { message = "Usuario registrado exitosamente." });
            }
            catch (Exception ex)
            {
                // Loggear el error (en un entorno real, usar un logger como Serilog)
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                return StatusCode(500, new { message = "Ocurrió un error al intentar registrar el usuario." });
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return string.Empty;
            }
            // Si el campo contiene comas, comillas dobles o saltos de línea,
            // debe encerrarse entre comillas dobles y las comillas dobles dentro del campo se duplican.
            if (field.Contains(",") || field.Contains("\"") || field.Contains(Environment.NewLine))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }
            return field;
        }
    }
}