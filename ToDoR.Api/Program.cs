using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ToDoR.DataAccess.Context;
using ToDoR.DataAccess.Interfaces;
using ToDoR.DataAccess.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDoR is Api with a RESTfull HTTP service",
        Version = "v1",
    });
});

builder.Services.AddCors(c =>
    c.AddPolicy("AllowOrigin", op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
);
string connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ToDoDbContext>(options => {
options.UseSqlServer(connString);
});

builder.Services.AddScoped<ToDoDbContext>(sp => {
    return new ToDoDbContext(new DbContextOptionsBuilder<ToDoDbContext>().UseSqlServer(connString).Options);
});

builder.Services.AddScoped<IDoTaskProvider, DoTaskProvider>();

var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
