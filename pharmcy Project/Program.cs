using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using pharmcy.Repository;
using pharmcy.Repository.Data;
using pharmcy.Repository.Identity;
using Pharmcy.Core.Entities.Identity;
using Pharmcy.Core.Repositories;
using Pharmcy.Core.Services;
using Pharmcy.Services;
using pharmcy_Project.Errors;
using pharmcy_Project.Extention;
using pharmcy_Project.Helpers;
using pharmcy_Project.Middlewares;
using StackExchange.Redis;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace pharmcy_Project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 
            builder.Services.AddDbContext<PharmcyContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var cinnections = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(cinnections);
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.Services.AddScoped<ITokenServices,TokenService>();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                     ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience=builder.Configuration["Jwt:ValidAudience"],
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))


                    };
                } );

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddApplicationServices();


            var app = builder.Build();
            #region UpdateDatabase

            //  PharmcyContext context = new PharmcyContext();
            //await  context.Database.MigrateAsync();
            using var Scope = app.Services.CreateScope();
            var services = Scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
              
                var dbContext = services.GetRequiredService<PharmcyContext>();
                await dbContext.Database.MigrateAsync();
                var identity = services.GetRequiredService<AppIdentityDbContext>();
                await identity.Database.MigrateAsync();
                var UserManager=services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityContextSeed.SeedUserAsync(UserManager);
                //await PharmcySeed.SeedData(dbContext);
            }
            catch(Exception ex)
            {
                var logger= loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }


            #endregion

           


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionmiddleWare>();


                app.useswaggerMidlleWares();
            }
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
