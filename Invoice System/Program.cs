using System.Text;
using Invoice_System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Invoice_System.Services;
using Invoice_System.profiles;
using Invoice_System.Helpers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy", x =>
        x.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

builder.Services.AddAutoMapper(typeof(AdminProfile));

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing");
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = issuer,
           ValidAudience = audience,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
       };
   });

// Add this after AddAuthentication()
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// Register services
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICashierService, CashierService>();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CustomPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();