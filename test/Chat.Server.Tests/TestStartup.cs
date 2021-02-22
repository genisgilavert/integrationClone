using Chat.Server.Library.Data;
using Chat.Server.Tests.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(Chat.Server.Startup).Assembly);
            //Resto de registro de dependencias

            //Registramos la dependencia de inmemory
            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<ChatDbContext>(options => {
                //options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
                options.UseInMemoryDatabase(databaseName: dbName);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat.Service.Api", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }



}
