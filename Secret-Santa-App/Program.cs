using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Secret_Santa_App.Data;
using Secret_Santa_App.EnvironmentSettings;
using Secret_Santa_App.GlobalErrorHandling;
using Secret_Santa_App.Services;
using Secret_Santa_App.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standard Authorization Header Using the Bearer Scheme (\"bearer {token}\")",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        
        }
    );
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddDbContext<DataContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("SecretSantaConnection")));


builder.Services.AddCors();

builder.Services.Configure<SecretConfiguration>(builder.Configuration.GetSection(nameof(SecretConfiguration)));
builder.Services.AddOptions();


//Authentication Middleware
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretConfiguration:SecretKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


//DI
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Custom exception Middleware (GLOBAL ERROR HANDLING)
app.ConfigureExceptionMiddleware();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();