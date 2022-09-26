using FluentValidation.AspNetCore;
using MiniTrade.Application.Services.LocalStorage;
using MiniTrade.Application.Validators.Products;
using MiniTrade.Infastructures;
using MiniTrade.Infastructures.Filters;
using MiniTrade.Persistence;

var builder = WebApplication.CreateBuilder(args);

//Generik Storage Altyapısı servisi dahil etme( Generic StorageService Registiration)
//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage(MiniTrade.Infastructures.Enums.StorageTypes.Azure);
// Add services to the container.
builder.Services.AddControllers(options=>options.Filters.Add<ValidationFilter>()).
  AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()).
  ConfigureApiBehaviorOptions(options=>options.SuppressModelStateInvalidFilter=true);
builder.Services.AddInfastructuresServices(); //infastructures servislerişnin eklenmesi
//fluent valdiation service registiration(fluent validasyon servis kayıdı işlemi)

//AddCors use for add cors policies(Cors Politikalarını eklemek için kullanılır)
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("https://localhost:4200",
  "http://localhost:4200/").AllowAnyHeader().AllowAnyMethod()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
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
