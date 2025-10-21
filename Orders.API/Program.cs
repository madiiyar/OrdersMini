using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Abstractions;
using Orders.Infrastructure.Mongo;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// EF Core (Postgres)
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// MediatR + FluentValidation
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("Orders.Application")));
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Orders.Application"));

// Repos/UoW
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

// Mongo audit
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("Mongo"));
builder.Services.AddSingleton<IOrderHistoryWriter, MongoOrderHistoryWriter>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
