using Hotel.Booking.Infrastructure.Database;
using Hotel.Booking.Application;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using Serilog;
using System;
using Hotel.Booking.Infrastructure;
using Hotel.Booking.Api.Middleware;
using Hotel.Booking.Application.Behaviors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Hotel.Booking.Application.Common.Interfaces;
using Hotel.Booking.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
//Add Serilog config
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddApplication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
});

// Keep default Swagger generation. Example providers were removed to avoid build errors.

//Add DbContext
builder.Services.AddDbContext<CardsBdContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HotelsDb")));
//Add DbConnectionFactory
builder.Services.AddScoped<IDbConnectionFactory>(provider =>
    new DbConnectionFactory(builder.Configuration.GetConnectionString("HotelsDb")));

builder.Services.AddInfrastructure(builder.Configuration);




//App FluentValidation and MediatR
builder.Services.AddValidatorsFromAssembly(
    typeof(Hotel.Booking.Application.AssemblyReference).Assembly);

//Add Health Checks
builder.Services.AddHealthChecks();

//Add OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracer =>
    {
        tracer
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    });

//Middleware de execpciones
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


// Aplicar Validation
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseMiddleware<TraceIdentifierMiddleware>();
app.UseMiddleware<Hotel.Booking.Api.Middleware.IdempotencyMiddleware>();
app.UseAuthorization();

app.MapControllers();

//Health Checks endpoint
app.MapHealthChecks("/health");

app.Run();
