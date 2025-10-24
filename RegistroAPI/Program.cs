var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // *** ASEGÚRATE DE INCLUIR AMBOS PROTOCOLOS Y EL PUERTO CORRECTO PARA TU FRONTEND ***
                          // Si tu HTML se carga desde el sistema de archivos (file:///), el origen es null
                          // Si lo abres con Live Server u otro, será http://localhost:XXXX o https://localhost:XXXX
                          // Incluyo ambos 7240 (HTTPS) y 5007 (HTTP) de la API por si acaso el frontend se sirviera desde alguno
                          policy.WithOrigins("https://localhost:7240", // Origen de tu API HTTPS
                                             "http://localhost:5007",  // Origen de tu API HTTP
                                             "http://localhost:7240",  // Si tu frontend se sirve desde este puerto HTTP
                                             "https://localhost:7240",
                                             "https://localhost:7024"// Si tu frontend se sirve desde este puerto HTTPS
                                             /* Puedes añadir otros orígenes aquí si tu frontend está en otro lugar */)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
// ************************************************
// Fin de la configuración de CORS

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build(); // Aquí se construye la aplicación y la colección de servicios se vuelve de solo lectura

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ************************************************
// Y el uso de CORS (app.UseCors) debe ir AQUÍ,
// después de app.UseRouting() si lo tuvieras, y antes de app.UseAuthorization()
// ************************************************
app.UseCors(MyAllowSpecificOrigins);
// ************************************************
// Fin del uso de CORS

app.UseAuthorization();

app.MapControllers();

app.Run();