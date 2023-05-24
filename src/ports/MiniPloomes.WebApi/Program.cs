using MiniPloomes.Core.Usuarios.Bootstrapper;
using MiniPloomes.Core.Usuarios.DataAccess.Migrations;
using MiniPloomes.Core.Usuarios.DataAccess.Repository;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("Usuarios");
builder.Services.AddSingleton<SqlConnectionStringBuilder>(sp =>
{
    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

    return builder;
});

builder.Services.AddScoped<IMigration, Migration0001_Initial>();
builder.Services.AddScoped<MigrationsBootstrapper>();

builder.Services.AddScoped<UsuarioRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrations = scope.ServiceProvider.GetRequiredService<MigrationsBootstrapper>();
    migrations.Executar();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
