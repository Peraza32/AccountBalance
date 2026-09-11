using CardAPI.Infrastructure.Persistance;
using CardAPI.Infrastructure.Repositories;
using CardAPI.Infrastructure.Repositories.Interfaces;
using CardAPI.Middleware;
using CardAPI.Utils.Profiles;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<CardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));
var connectionString = builder.Configuration.GetConnectionString("SQLConnection");

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ILogsRepository, LogsRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();

//Automapper 
builder.Services.AddAutoMapper(typeof(PurchaseProfile));
builder.Services.AddAutoMapper(typeof(PaymentProfile));
builder.Services.AddAutoMapper(typeof(ClientProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: connectionString,
        healthQuery: "SELECT 1;",
        name: "sql-server",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql" });

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
