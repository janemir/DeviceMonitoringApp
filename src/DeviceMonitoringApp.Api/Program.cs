using DeviceMonitoringApp.Infrastructure.Extensions;
using DeviceMonitoringApp.Infrastructure.Repositories;
using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Application.Services;
using DeviceMonitoringApp.Middlewares;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.MigrateDatabase(builder.Configuration);
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
// builder.WebHost.UseUrls("http://*:80");

var app = builder.Build();

//app.UseCors("AllowFrontend");
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();