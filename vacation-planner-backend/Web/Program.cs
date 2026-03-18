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
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Services
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// DI
builder.Services.AddScoped<IVacationRequestService, VacationRequestService>();
builder.Services.AddScoped<IVacationRequestRepository, VacationRequestRepository>();
builder.Services.AddScoped<IBaseUow, BaseUow<AppDbContext>>();
builder.Services.AddScoped<IMapper<VacationRequest, VacationRequestEntity>, VacationRequestMapper>();
builder.Services.AddScoped<IMapper<VacationRequestDto, VacationRequest>, VacationRequestDtoMapper>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:8080")
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