using EduDataAPI;
using EduDataAPI.Modelos;
using EduDataAPI.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Controladores
builder.Services.AddControllers();


// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Repositorios
builder.Services.AddScoped<UsuarioRepository>();


// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// Sin SSL
//app.UseHttpsRedirection();


app.MapControllers();

app.Run();