using Asp.Versioning;
using backend.Business.Implementations;
using backend.Business;
using backend.Model;
using backend.Model.Context;
using backend.Repository.Generic;
using backend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var appName = "API Dotnet";
var appVersion = "v1";
var appDescription = $"C# MVC .NET8 RESTful developed";


// Database connect
var connection = builder.Configuration["SQLConnection:SQLConnection"];
builder.Services.AddDbContext<SQLContext>(options => options.UseSqlServer(connection));

//Versioning API
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});

// Add services
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(appVersion,
        new OpenApiInfo
        {
            Title = appName,
            Version = appVersion,
            Description = appDescription,
            Contact = new OpenApiContact
            {
                Name = "GitHub",
                Url = new Uri("https://github.com/codevenger/api_dotnet")
            }
        });
});

//Dependency Injection
builder.Services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var sqlContext = scope.ServiceProvider.GetRequiredService<SQLContext>();
            sqlContext.Database.Migrate();

            if (sqlContext.Users.FirstOrDefault() == null)
            {
                string user = "admin";
                string pass = "senha123";
                IdentityUser identityUser = new IdentityUser() { UserName = user };
                PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
                string hashPassword = hasher.HashPassword(identityUser, pass);

                sqlContext.Add(new User
                {
                    Username = user,
                    Fullname = "Administrador",
                    Password = hashPassword
                });
                sqlContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Log.Error("Database migration failed", ex);
            throw;
        }
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
