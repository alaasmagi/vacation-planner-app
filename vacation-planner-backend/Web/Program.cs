using Application;
using Base.Contracts.DataAccess;
using Base.Contracts.DTO;
using Base.DataAccess.EF;
using Contract.Application;
using Contract.DataAccess;
using DataAccess;
using Domain;
using DTO.DataAccess;
using DTO.DataAccess.Mappers;
using DTO.Presentation;
using DTO.Presentation.Mappers;
using Helpers;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load("../.env");
var builder = WebApplication.CreateBuilder(args);

var envInitializer = new EnvInitializer();
envInitializer.InitializeEnv();

if (envInitializer.BackendUrl != string.Empty)
{
    builder.WebHost.UseUrls(envInitializer.BackendUrl);
}

// Services
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
var connectionString = builder.Environment.IsDevelopment()
    ? envInitializer.DbConnectionDevelopment
    : envInitializer.DbConnection;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// DI
builder.Services.AddSingleton(envInitializer);
builder.Services.AddScoped<IVacationRequestService, VacationRequestService>();
builder.Services.AddScoped<IVacationRequestRepository, VacationRequestRepository>();
builder.Services.AddScoped<IBaseUow, BaseUow<AppDbContext>>();
builder.Services.AddScoped<IMapper<VacationRequest, VacationRequestEntity>, VacationRequestMapper>();
builder.Services.AddScoped<IMapper<VacationRequestDto, VacationRequest>, VacationRequestDtoMapper>();
builder.Services.AddScoped<IMapper<VacationRequestDto, VacationRequestWebDto>, VacationRequestWebDtoMapper>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(envInitializer.FrontendUrl)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
