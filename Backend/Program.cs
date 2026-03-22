using Backend.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Database Connection Add Kiya
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<Backend.Domain.Interfaces.IUnitOfWork, Backend.Infrastructure.Repositories.UnitOfWork>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly); // FluentValidation ke liye
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// 2. MediatR (CQRS) Add Kiya
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Backend.Features.Behaviors.ValidationBehavior<,>));
});

// 3. CORS Add Kiya (Taki React Frontend API call kar sake)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Vite React ka default port
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// --- JWT AUTHENTICATION SETUP
var jwtSecret = builder.Configuration["JwtSettings:Secret"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret!)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Swagger JWT Authorization Setup
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });

    // "Authorize" button add karne ke liye
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token.\r\n\r\nExample: \"Bearer eyJhbGci...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GLOBAL EXCEPTION HANDLER (FluentValidation ke errors ko clean response me badalne ke liye)
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (FluentValidation.ValidationException ex)
    {
        context.Response.StatusCode = 400; // Bad Request
        context.Response.ContentType = "application/json";

        var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage });
        await context.Response.WriteAsJsonAsync(new { Message = "Validation Failed", Errors = errors });
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500; // Internal Server Error
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
    }
});

// CORS Middleware Add Kiya (Authorization se pehle zaroori hai)
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();