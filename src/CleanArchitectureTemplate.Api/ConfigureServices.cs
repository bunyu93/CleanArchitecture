using System.Reflection;

namespace CleanArchitectureTemplate.Api
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();

            return services;
        }
    }
}