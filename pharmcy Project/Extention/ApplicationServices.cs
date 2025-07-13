using Microsoft.AspNetCore.Mvc;
using pharmcy.Repository;
using Pharmcy.Core.Repositories;
using pharmcy_Project.Errors;
using pharmcy_Project.Helpers;

namespace pharmcy_Project.Extention
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        
            {
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddSingleton(typeof(PdfGenerator));
            services.AddAutoMapper(typeof(MappingProfile));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => e.Value.Errors.First().ErrorMessage)
                        .ToList();
                    var apiValidationError = new ApiValidationError
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(apiValidationError);
                };
            });
          services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            return services;
        }
    }
}
