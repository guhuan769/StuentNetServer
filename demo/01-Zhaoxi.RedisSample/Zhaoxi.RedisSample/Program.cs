using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh-cn");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>();
builder.Services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(redisConfig);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseRedisInformation();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRedisInformation();

app.MapControllers();

app.Run();
