using Discount.API.Repositories;
using Discount.API.Repositories.Interfacies;

namespace Discount.Grpc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with Grpc endpoints must be made thrw");
                });
            });
        }
    }
}