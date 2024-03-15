

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
var isDev = builder.Environment.IsDevelopment();
isDev = false;
if (!isDev)
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect("Endpoint=https://central-configuration.azconfig.io;Id=KG8b;Secret=gcJVBJG4TEgZK2zGmGYmFqrzp/fxpYGgCgZVd/Z/OtA=");
    });
}
// Add services to the container.
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddShared(builder.Configuration);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "abc",
       builder =>
       {
           builder.WithOrigins("http://localhost:3000")
              .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
       });
});
RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
string? publicPrivateKey = builder.Configuration["PublicPrivateKey"];

provider.FromXmlString(publicPrivateKey);

RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(provider);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        IssuerSigningKey = rsaSecurityKey
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("abc");
app.UseAuthorization();

app.MapControllers();

app.Run();
