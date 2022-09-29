using ETicaretAPI.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MiniTrade.Application;
using MiniTrade.Application.Validators.Products;
using MiniTrade.Infastructures;
using MiniTrade.Infastructures.Services.Storage.Azure;
using MiniTrade.Persistence;
using System.Text;

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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer("admin", options =>
  options.TokenValidationParameters = new()
  {
    ValidateAudience=true, // hangi originlerin/sitelerin kullanacağını belirlediğimiz değerdir
    ValidateIssuer=true,//oluşturulucak token değerini kimin dağıttığını ifade edilicek alan
    ValidateLifetime=true,//oluşturulan tokenın yaşam süresini kontrol eder
    ValidateIssuerSigningKey=true,//oluşturulan tokenın uygulamamıza ait bir değer olduğunu ifade eden security key verisini doğrular
    ValidAudience = builder.Configuration["JWT:Audience"],
    ValidIssuer= builder.Configuration["JWT:Issuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"]) ),

  }
  );
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
