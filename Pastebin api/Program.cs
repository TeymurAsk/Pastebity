using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Pastebin_api.Controllers;
using Pastebin_api.Data;
using Pastebin_api.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<S3Service>();
builder.Services.AddScoped<HashGenerator>();
builder.Services.AddScoped<S3Controller>();
builder.Services.AddScoped<MainController>();
builder.Services.AddScoped<RedisService>();
builder.Services.AddScoped<RedisCleanupWorker>();

// Redis Services (first cache, then database for tasks)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConn");
    options.InstanceName = "Pastebin_";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConn"));
});
// ----------------------------------------------------


builder.Services.AddDbContext<PastebinDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});
// Registration of backgroud worker
builder.Services.AddHostedService<RedisCleanupWorker>();

var app = builder.Build();

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
