
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieClubX.Data;
using MovieClubX.Endpoint.Helpers;
using MovieClubX.Entities.Entity;
using MovieClubX.Logic;
using MovieClubX.Logic.Dto;
using System.Text;

namespace MovieClubX.Endpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieClub API", Version = "v1" });
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
            });

            builder.Services.AddTransient(typeof(Repository<>));
            builder.Services.AddTransient<MovieLogic>();
            builder.Services.AddTransient<DtoProvider>();

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MovieClubContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "movieclub.com",
                    ValidIssuer = "movieclub.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Nagyonhossz�titkos�t�kulcsNagyonhossz�titkos�t�kulcsNagyonhossz�titkos�t�kulcsNagyonhossz�titkos�t�kulcsNagyonhossz�titkos�t�kulcsNagyonhossz�titkos�t�kulcs"))
                };
            });

            builder.Services.AddDbContext<MovieClubContext>(opt =>
            {
                //macen: Sqlite (nuget csomag is kell hozz�)
                opt
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MovieClubDbX;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
                .UseLazyLoadingProxies();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
           

            app.MapControllers();

            app.Run();
        }
    }
}
