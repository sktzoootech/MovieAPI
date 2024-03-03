using MovieApi.Policies;

namespace MovieApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddHttpClient("Fetch").AddPolicyHandler(
                request => request.Method == HttpMethod.Get ? new ClientPolicy().ExponentialHttpRetry : new ClientPolicy().ExponentialHttpRetry);
            
            services.AddSingleton<ClientPolicy>(new ClientPolicy());
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://170.64.165.233");
                });
            });

            return services;
        }
    }
}