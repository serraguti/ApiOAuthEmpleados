using ApiOAuthEmpleados.Data;
using ApiOAuthEmpleados.Helpers;
using ApiOAuthEmpleados.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
//PONEMOS EL HELPER EN LA INYECCION
builder.Services.AddSingleton<HelperOAuthToken>();
HelperOAuthToken helper = new HelperOAuthToken(builder.Configuration);
//AÑADIMOS LAS OPCIONES DE AUTENTIFICACION
builder.Services.AddAuthentication(helper.GetAuthenticationOptions())
    .AddJwtBearer(helper.GetJwtOptions());

// Add services to the container.
string connectionString =
    builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddTransient<RepositoryEmpleados>();
builder.Services.AddDbContext<EmpleadosContext>
    (options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api OAuth Empleados 2023", Version = "v1"
        , Description = "Ejemplo Seguridad Api Tokens"
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json"
        , "Api OAuth Empleados");
    options.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
