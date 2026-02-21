using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StarshipShop.Api.Data;
using StarshipShop.Api.Middleware;
using StarshipShop.Api.Routers;
using StarshipShop.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add JWT authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStarshipService, StarshipService>();
builder.Services.AddScoped<IEngineService, EngineService>();
builder.Services.AddScoped<IFtlDriveService, FtlDriveService>();

// Add OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Add exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Auto-migrate database in Development
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Map route groups
var authGroup = app.MapGroup("/api/auth")
    .WithTags("Authentication");
authGroup.MapAuthRoutes();

var starshipsGroup = app.MapGroup("/api/starships")
    .RequireAuthorization()
    .WithTags("Starships");
starshipsGroup.MapStarshipRoutes();

var enginesGroup = app.MapGroup("/api/engines")
    .RequireAuthorization()
    .WithTags("Engines");
enginesGroup.MapEngineRoutes();

var ftlDrivesGroup = app.MapGroup("/api/ftl-drives")
    .RequireAuthorization()
    .WithTags("FtlDrives");
ftlDrivesGroup.MapFtlDriveRoutes();

app.Run();

