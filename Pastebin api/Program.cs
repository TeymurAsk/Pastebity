using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pastebin_api.Controllers;
using Pastebin_api.Data;
using Pastebin_api.Extensions;
using Pastebin_api.Services;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Pastebin API", 
        Version = "v1",
        Description = "An API to perform Pastebin operations(login or register beforehand)",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Creator",
            Email = "teymurasgar@gmail.com",
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
// Amazor S3 storage

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<S3Service>();

// Controllers

builder.Services.AddScoped<S3Controller>();
builder.Services.AddScoped<MainController>();
builder.Services.AddScoped<AuthController>();

// Services for authentication
builder.Services.AddScoped<HashGenerator>();
builder.Services.AddScoped<JWTProvider>();
builder.Services.AddScoped<AuthService>();

//Redis cache and db services
builder.Services.AddScoped<RedisService>();
builder.Services.AddScoped<RedisCleanupWorker>();

// That's needed for some reason (don't know why it's not built-in)
builder.Services.AddHttpContextAccessor();

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

// yeah, I don't know what to say about that...
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection(nameof(JWTOptions)));

builder.Services.AddDbContext<PastebinDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});
// Registration of backgroud worker
builder.Services.AddHostedService<RedisCleanupWorker>();

// Register API extensio
builder.Services.AddApiAuthentication(
    builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JWTOptions>>()
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pastebin API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
