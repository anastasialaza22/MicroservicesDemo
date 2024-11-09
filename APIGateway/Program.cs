using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление конфигурации Ocelot
builder.Configuration.AddJsonFile("ocelot.json");

// Настройка аутентификации JWT
var key = Encoding.ASCII.GetBytes("SuperSecretKey12345");
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
    });

// Добавление Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Добавление контроллеров
builder.Services.AddControllers();

// Добавление Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
});

var app = builder.Build();

// Включение Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway V1");
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Использование Ocelot Middleware
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.UseOcelot();

app.Run();