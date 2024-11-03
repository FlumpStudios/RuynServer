using Microsoft.EntityFrameworkCore;
using RuynServer.Data;
using RuynServer.MIddleware;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RuynServerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RuynServerContext") ?? throw new InvalidOperationException("Connection string 'RuynServerContext' not found.")));

// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{ 
    app.UseMiddleware<AuthMiddleware>();
    app.UseAuthorization();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
