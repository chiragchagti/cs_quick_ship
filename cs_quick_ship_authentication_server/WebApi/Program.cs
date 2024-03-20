
using Domain.Configuration;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Models.Context;
using Infrastructure.Services;
using Application.Interfaces;
using cs_quick_ship_authentication_server.Services;
using cs_quick_ship_authentication_server.Validation;
using cs_quick_ship_authentication_server.Services.CodeServce;
using cs_quick_ship_authentication_server.Services.Configuration;
using Application;

var builder = WebApplication.CreateBuilder(args);
var configServices = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("BaseDBConnection");
builder.Services.AddDbContext<BaseDBContext>(op =>
{
    op.UseSqlServer(connectionString);
});

var isDev = builder.Environment.IsDevelopment();
isDev = false;
if (!isDev)
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect("Endpoint=https://central-configuration.azconfig.io;Id=KG8b;Secret=gcJVBJG4TEgZK2zGmGYmFqrzp/fxpYGgCgZVd/Z/OtA=");
    });
}
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<BaseDBContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login";
    options.AccessDeniedPath = "/Accounts/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
});


builder.Services.Configure<OAuthServerOptions>(configServices.GetSection("OAuthOptions"));
builder.Services.AddScoped<IAuthorizeResultService, AuthorizeResultService>();
builder.Services.AddScoped<ICodeStoreService, CodeStoreService>();
builder.Services.AddSingleton<ConcurrentDictionaryService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>();
builder.Services.AddScoped<IBearerTokenUsageTypeValidation, BearerTokenUsageTypeValidation>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
//Cors
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("abc");
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
//app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
