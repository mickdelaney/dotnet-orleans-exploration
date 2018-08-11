using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client
{
    public class Startup
    {
        public Startup(ILogger<Startup> logger, IHostingEnvironment hostingEnvironment)
        {
            Logger = logger;
            HostingEnvironment = hostingEnvironment;
        }

        public ILogger<Startup> Logger { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
        }
        
    }
}