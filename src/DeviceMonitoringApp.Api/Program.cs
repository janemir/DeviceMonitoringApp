using DeviceMonitoringApp.Infrastructure.Extensions;
using DeviceMonitoringApp.Infrastructure.Repositories;
using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Application.Services;
using DeviceMonitoringApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.MigrateDatabase(builder.Configuration);
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
// builder.WebHost.UseUrls("http://*:80");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();