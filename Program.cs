using DotNetWebApiProject.Data;
using Microsoft.EntityFrameworkCore;

using DotNetWebApiProject.Cache;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<DotNetWebApiProject.Repositories.IUserRepository, DotNetWebApiProject.Repositories.UserRepository>();
builder.Services.AddScoped<DotNetWebApiProject.Services.IUserService, DotNetWebApiProject.Services.UserService>();

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.AddSingleton<ICacheService, RedisCacheService>();

builder.Services.AddControllers(); // Add this line to register controllers

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // Add this line to map controllers

app.Run();