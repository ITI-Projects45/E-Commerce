
using System.Text;
using Azure.Core;
using E_Commerce.DAL;
using E_Commerce.DB;
using E_Commerce.DB.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace E_Commerce
{
    // this is main
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddControllers();
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true; 
            }); ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            // تسجيل الcore من اجل الاتصالات الخارجية 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                    policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
                );
            });

            // Register DataBase
            builder.Services.AddDbContext<DataBaseContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Cs"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<DataBaseContext>()
               .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            #region Add Auth Configuration

            builder.Services.AddAuthentication(option =>{
                // باعمل حقن للjwt as meddilware 
                //الخيارات دي علشان اقول للركوست الي جاي 
                // اتاكد بطريةق ال jwt 
                // تحقق بطريقة ال jwt
                // اتصرف بطريقة ال jwt 
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // لو فية توكين شيك علي الbearer token
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // لو مافيش توكين رجع 401 unauth
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                // من الاخر , استخدم ال jwt علشان تتاكد من كل ركوست جاي 
            }).AddJwtBearer(option =>
            {
                // يحتفظ بالتوكين في الهيدر علشان لو حبيت استعملها تاني بعدين 
                option.SaveToken = true;
                
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Iss"],//proivder
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Aud"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };

            });

            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(swagger =>
            {

                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = " ITI Projrcy"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                });
            
            });


            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
           // if (app.Environment.IsDevelopment())
           // {
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseStaticFiles(); //read image  

            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
