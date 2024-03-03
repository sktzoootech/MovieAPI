using System.Reflection;
using MovieApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Configuration.Sources.Clear();
// builder.Configuration
//     .AddJsonFile("appsettings.json")
//     .AddJsonFile("appsettings.Development.json", false)
//     .AddUserSecrets(Assembly.GetEntryAssembly()!)
//     .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
