using Microsoft.AspNetCore.Mvc;
using RegistroAPI.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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
                // Loggear el error
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                return StatusCode(500, new { message = "Ocurrió un error al intentar registrar el usuario." });
            }
        }

        [HttpPost("login")] // Define una ruta específica para el login
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validación simple de campos vacíos
            if (string.IsNullOrEmpty(request.NombreUsuario) || string.IsNullOrEmpty(request.Contrasena))
            {
                return BadRequest(new { message = "Se requiere nombre de usuario y contraseña." });
            }

            try
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    return Unauthorized(new { message = "No hay usuarios registrados." });
                }

                // Leer todas las líneas del archivo CSV
                var lines = await System.IO.File.ReadAllLinesAsync(_filePath, Encoding.UTF8);

                // Saltar la primera línea (encabezado)
                foreach (var line in lines.Skip(1))
                {
                    var fields = ParseCsvLine(line); // Usar una función auxiliar para parsear CSV
                    if (fields.Count >= 5) // Asegurarse de que hay suficientes campos
                    {
                        // Los índices corresponden a: 0:Fecha, 1:NombreCompleto, 2:Correo, 3:NombreUsuario, 4:Contrasena
                        var storedNombreUsuario = fields[3];
                        var storedContrasena = fields[4];

                        if (storedNombreUsuario == request.NombreUsuario && storedContrasena == request.Contrasena)
                        {
                            // Autenticación exitosa
                            return Ok(new { message = "Inicio de sesión exitoso.", redirectTo = "/Home/Habitaciones" });
                        }
                    }
                }

                // Si se recorrió todo el archivo y no se encontró el usuario
                return Unauthorized(new { message = "Credenciales incorrectas." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante el login: {ex.Message}");
                return StatusCode(500, new { message = "Ocurrió un error en el servidor durante el inicio de sesión." });
            }
        }

        // Función auxiliar para parsear una línea CSV de forma robusta
        private List<string> ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var inQuote = false;
            var currentField = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (c == '"')
                {
                    if (inQuote && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Es una comilla doble escapada
                        currentField.Append('"');
                        i++; // Saltar la siguiente comilla
                    }
                    else
                    {
                        inQuote = !inQuote;
                    }
                }
                else if (c == ',' && !inQuote)
                {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }
            }
            fields.Add(currentField.ToString()); // Añadir el último campo

            return fields;
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return string.Empty;
            }
            if (field.Contains(",") || field.Contains("\"") || field.Contains(Environment.NewLine))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }
            return field;
        }
    }
}