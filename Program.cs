using AlHudhud.Data;
using AlHudhud.Models;
using AlHudhud.Services.AuthService;
using AlHudhud.Services.EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// CORS Config
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:4200",
                "https://alhudhud.masarak.app" // Vite
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// HttpContextAccessor for injecting cookies from services
builder.Services.AddHttpContextAccessor();

// AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

// JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
        RoleClaimType = ClaimTypes.Role
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["access_token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        var policyName = context.HttpContext.Items["RateLimitPolicy"]?.ToString();

        if (policyName == "AuthLimiter")
        {
            await context.HttpContext.Response.WriteAsync(
                "Too many login attempts. Please try again later.",
                token);
        }
        //else if (policyName == "GeneralRateLimiter")
        //{
        //    await context.HttpContext.Response.WriteAsync(
        //        "Too many requests. Please slow down.",
        //        token);
        //}
    };

    options.AddPolicy("AuthLimiter", httpContext =>
    {
        httpContext.Items["RateLimitPolicy"] = "AuthLimiter";

        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            ip,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1)
            });
    });

    //options.AddPolicy("GeneralRateLimiter", httpContext =>
    //{
    //    httpContext.Items["RateLimitPolicy"] = "GeneralRateLimiter";

    //    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    //    return RateLimitPartition.GetFixedWindowLimiter(
    //        ip,
    //        _ => new FixedWindowRateLimiterOptions
    //        {
    //            PermitLimit = 3,
    //            Window = TimeSpan.FromMinutes(1)
    //        });
    //});
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");


app.UseRateLimiter(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Admin User on startup
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        await AlHudhud.Data.DbSeeder.SeedAdminUserAsync(services);
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while seeding the Admin user.");
//    }
//}

app.Run();
