using ETicaretAPI.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using MiniTrade.API.Configurations.ColumnWriter;
using MiniTrade.API.Extensions;
using MiniTrade.Application;
using MiniTrade.Application.Validators.Products;
using MiniTrade.Infastructures;
using MiniTrade.Infastructures.Services.Storage.Azure;
using MiniTrade.Persistence;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using SignalR;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();
//Generik Storage Altyapısı servisi dahil etme( Generic StorageService Registiration)
builder.Services.AddStorage<AzureStorage>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("https://localhost:4200",
  "http://localhost:4200/").AllowAnyHeader().AllowAnyMethod()));
//Seri log servis kayıtı(serilog service reg)
SqlColumn sqlColumn = new SqlColumn();
sqlColumn.ColumnName = "UserName";
sqlColumn.DataType = System.Data.SqlDbType.NVarChar;
sqlColumn.PropertyName = "UserName";
sqlColumn.DataLength = 50;
sqlColumn.AllowNull = true;
ColumnOptions columnOpt = new ColumnOptions();
columnOpt.Store.Remove(StandardColumn.Properties);
columnOpt.Store.Add(StandardColumn.LogEvent);
columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };
Logger log = new LoggerConfiguration().
  WriteTo.Console().
  WriteTo.File("logs/log.txt").
  WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sinkOptions: new MSSqlServerSinkOptions
  {
    AutoCreateSqlTable = true,
    TableName = "logs",
  }, appConfiguration: null,
     columnOptions: columnOpt
  ).WriteTo.Seq(builder.Configuration["Seq:ServerUrl"],apiKey: builder.Configuration["Seq:ApiKey"])
  .Enrich.FromLogContext()
    .Enrich.With<CustomUserNameColumn>()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
//infastructures servislerişnin eklenmesi
//fluent valdiation service registiration(fluent validasyon servis kayıdı işlemi)
builder.Services.AddHttpLogging(logging =>//htttp loggni process
{
  logging.LoggingFields = HttpLoggingFields.All;
  logging.RequestHeaders.Add("sec-ch-ua");
  logging.ResponseHeaders.Add("MyResponseHeader");
  logging.MediaTypeOptions.AddText("application/javascript");
  logging.RequestBodyLogLimit = 4096;
  logging.ResponseBodyLogLimit = 4096;

});

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
    LifetimeValidator = ( notBefore,  expires,  securityToken, validationParameters) =>expires!=null?expires>DateTime.Now:false,

    NameClaimType=ClaimTypes.Name // JWT üzerinde name claimine karşılık gelen değeri elde etmeye yarar

  }
  );
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseAppExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
  var userName = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
  LogContext.PushProperty("UserName", userName);
  await next();
});

app.MapControllers();
app.MapHubs();
app.Run();
