using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace routing_netcore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("is-albert", typeof(OnlyAlbertsConstraint));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //ROUTING BÁSICO
            //var routeBuilder = new RouteBuilder(app);

            //routeBuilder.MapGet("/hola", async context =>
            //{
            //    await context.Response.WriteAsync("Hola");
            //});

            //routeBuilder.MapPost("/people/update/{id}", async context =>
            //{
            //    var id = context.GetRouteValue("id");
            //    var name = context.Request.Query["name"];
            //    //simulamos actualizar bbdd
            //    await context.Response.WriteAsync($"Usuario actualizado {id}={name}");
            //});

            //app.UseRouter(routeBuilder.Build());

            //ENDPOINT ROUTING
            app.UseRouting();

            app.Use(async(context, next) =>
            {
                var selectedEndPoint = context.GetEndpoint();
                if (selectedEndPoint?.DisplayName == "Hello endpoint")
                {
                    var name = context.GetRouteValue("name")?.ToString();
                    if("Jon".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        context.Request.RouteValues["name"] = "John";
                    }
                }
                await next();
            });

            app.UseEndpoints(endpoints => 
            {
                //endpoints.MapGet("/hello/{name:is-albert}", async context => //restriccion propia (OnlyAlbertsConstraint)
                endpoints.MapGet("/hello/{name:string}", async context => //restricción genérica :string
                {
                    var name = context.GetRouteValue("name");
                    await context.Response.WriteAsync($"Hello {name}");
                }).WithDisplayName("Hola endpoint");
;            });
        }
    }
}
