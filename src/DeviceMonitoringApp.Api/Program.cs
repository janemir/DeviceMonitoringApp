using System.Reflection;
using DeviceMonitoringApp.Infrastructure.Extensions;
using ShopWebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.MigrateDatabase(builder.Configuration);
//builder.Services.AddRepositories();
//builder.Services.AddServices();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.WebHost.UseUrls("http://*:80");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();