using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoR.DataAccess.Context;
using ToDoR.DataAccess.Interfaces;
using ToDoR.DataAccess.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(c =>
    c.AddPolicy("AllowOrigin", op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
);

string mode = "Data Source=localhost;Initial Catalog=to_do_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


builder.Services.AddDbContext<ToDoDbContext>(options => {
options.UseSqlServer(mode);
});

builder.Services.AddScoped<ToDoDbContext>(sp => {
    return new ToDoDbContext(new DbContextOptionsBuilder<ToDoDbContext>().UseSqlServer(mode).Options);
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
