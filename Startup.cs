using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Repositories;
using Plantilla_Agenda.Servicios;

namespace Plantilla_Agenda  // Corrige el espacio de nombres aquí
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<AgendaRepository>();
            services.AddSingleton(new ContextoDB(Configuration.GetConnectionString("NombreConexion")));
            services.AddScoped<ServicioRepository>();
            services.AddScoped<AgendamientoRepository>();
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<PersonaRepository>();
            services.AddScoped<AuthenticationsService>();
            services.AddScoped<SedeRepository>();

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
                // Configuración de enrutamiento
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Rutas específicas para Agendamiento
                endpoints.MapControllerRoute(
                    name: "Agendamiento",
                    pattern: "Agendamiento/{action}/{id?}",
                    defaults: new { controller = "Agendamiento" });

                // Rutas específicas para Agenda
                endpoints.MapControllerRoute(
                    name: "Agenda",
                    pattern: "Agenda/{action}/{id?}",
                    defaults: new { controller = "Agenda" });

                // Rutas específicas para Servicios
                endpoints.MapControllerRoute(
                    name: "Servicio",
                    pattern: "Servicio/{action}/{id?}",
                    defaults: new { controller = "Servicio" });
            });
        }
    }
}
