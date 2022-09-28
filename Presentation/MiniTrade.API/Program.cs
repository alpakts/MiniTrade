using ETicaretAPI.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using MiniTrade.Application;
using MiniTrade.Application.Validators.Products;
using MiniTrade.Infastructures;
using MiniTrade.Infastructures.Services.Storage.Azure;
using MiniTrade.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

//Generik Storage Altyapısı servisi dahil etme( Generic StorageService Registiration)
builder.Services.AddStorage<AzureStorage>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("https://localhost:4200",
  "http://localhost:4200/").AllowAnyHeader().AllowAnyMethod()));


builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
//infastructures servislerişnin eklenmesi
//fluent valdiation service registiration(fluent validasyon servis kayıdı işlemi)

//AddCors use for add cors policies(Cors Politikalarını eklemek için kullanılır)

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
