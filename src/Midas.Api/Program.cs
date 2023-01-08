using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Midas.Api.Middleware;
using Midas.Auth.Core.Contracts;
using Midas.Auth.Infrastructure.Contract;
using Midas.Auth.Infrastructure.Options;
using Midas.Core;
using Midas.Core.Contracts;
using Midas.Infrastructure.Contracts;
using Midas.Infrastructure.Options;
using Midas.Users.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSystemsManager($"/Midas/{builder.Environment.EnvironmentName}");

builder.Services.Configure<PasswordHashingOptions>(builder.Configuration.GetSection("Security:Passwords"));
builder.Services.Configure<RefreshTokensOptions>(builder.Configuration.GetSection("Security:RefreshTokens"));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Version = "v1",
        Title = "Midas API",
    });
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put ",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var jwtSecret = builder.Configuration.GetValue<string>("Security:AccessTokens:JwtSecret") ?? string.Empty;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromSeconds(5),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        };
    });

builder.Services.AddAutoMapper(
    options => options.AddMaps("Midas.Api", "Midas.Users.Core")
);

builder.Services.AddTransient<ExceptionsHandlingMiddleware>();
builder.Services.AddScoped<CreateUserService>();
builder.Services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddDbContext<MidasContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Core");
    options.UseNpgsql(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionsHandlingMiddleware>();
app.Run();