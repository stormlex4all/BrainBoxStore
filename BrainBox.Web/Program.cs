using BrainBox.API.DependencyInjections;
using BrainBox.Core.Configuration;
using BrainBox.Data;
using BrainBox.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using POSSAP.Core.Configuration;
using BrainBox.Data.DTOs;

var builder = WebApplication.CreateBuilder(args);

//configure logger
log4net.GlobalContext.Properties["LoggerFilePath"] = builder.Environment.ContentRootPath;
builder.Logging.ClearProviders(); 
builder.Logging.AddLog4Net("log4net.config");

IConfiguration configuration = builder.Configuration;

//configure dbcontext with SQL
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//Adding Identity
builder.Services.AddIdentity<BrainBoxUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"])),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    RequireExpirationTime = true,
    ClockSkew = TimeSpan.Zero
};

// Add services to the container.
builder.Services.AddOptions();
builder.Services.Configure<APISettings>(builder.Configuration.GetSection("APISettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.TryAddSingleton<IJwtSettings>(x => x.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.TryAddSingleton<IAPISettings>(x => x.GetRequiredService<IOptions<APISettings>>().Value);
builder.Services.TryAddScoped<ICurrentActiveToken>(x => x.GetRequiredService<IOptions<CurrentActiveToken>>().Value);

//add other services
builder.Services.AddDependencyServices();

//Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//Add Jwt bearer
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddControllers();
builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}); builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//disable auto model validation error response in ApiController
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    // include xml comments
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BrainBox Store");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

//Authentication and authorization
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
