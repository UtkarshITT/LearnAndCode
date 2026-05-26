using System.Text;
using FinanceManager.Infrastructure;
using FinanceManager.Infrastructure.Configuration;
using FinanceManager.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("logs/finance-manager-.log", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSettings = new JwtSettings
{
	SecretKey = jwtSection["SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is required."),
	Issuer = jwtSection["Issuer"] ?? "FinanceManager",
	Audience = jwtSection["Audience"] ?? "FinanceManagerClient",
	ExpirationMinutes = int.Parse(jwtSection["ExpirationMinutes"] ?? "60")
};

if (!builder.Environment.IsEnvironment("Testing"))
{
	builder.Services.AddFinanceManagerInfrastructure(connectionString, jwtSettings);
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
			ValidateIssuer = true,
			ValidIssuer = jwtSettings.Issuer,
			ValidateAudience = true,
			ValidAudience = jwtSettings.Audience,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<FinanceManagerDbContext>();
	await db.Database.MigrateAsync();
}

app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
