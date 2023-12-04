using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Repositories;

namespace AgendaEmpresa
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<  AgendaRepository>();
            services.AddSingleton(new ContextoDB("tu cadena de conexión aquí"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = "/Persona/Login";
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Agendamiento",
                    pattern: "/Agendamiento/{controller=Agendamiento}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Agenda",
                    pattern: "/Agenda/{controller=Agenda}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Servicios",
                    pattern: "/Servicios/{controller=Servicios}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Home}/{action=Index}/{id?}");
            });

         

             
        }
    }
}
