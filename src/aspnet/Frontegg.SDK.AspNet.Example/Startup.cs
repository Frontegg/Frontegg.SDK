using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Proxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Frontegg.SDK.AspNet.Example
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
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
            
            services.TryAddSingleton<IFronteggProxyInfoExtractor, X>();
            
            services.AddFrontegg();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseFrontegg<X>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public class X : IFronteggProxyInfoExtractor
    {
        public Task<FronteggTenantInfo> Extract(HttpRequest request)
        {
            return Task.FromResult(new FronteggTenantInfo()
            {
                UserId = "my-user-id",
                TenantId = "my-tenant-id"
            });
        }
    }
}