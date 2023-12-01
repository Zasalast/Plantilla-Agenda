using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AgendaRepository, AgendaRepository>();
builder.Services.AddSingleton(new ContextoDB(builder.Configuration.GetConnectionString("Conexiondb")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Persona/Login";
    });

var app = builder.Build();

// Configuración del pipeline de solicitud
if (app.Environment.IsDevelopment())
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

// Configuración de enrutamiento
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rutas específicas para Agendamiento
app.MapControllerRoute(
    name: "Agendamiento",
    pattern: "Agendamiento/{action}/{id?}",
    defaults: new { controller = "Agendamiento" });

// Rutas específicas para Agenda
app.MapControllerRoute(
    name: "Agenda",
    pattern: "Agenda/{action}/{id?}",
    defaults: new { controller = "Agenda" });

// Rutas específicas para Servicios
app.MapControllerRoute(
    name: "Servicios",
    pattern: "Servicios/{action}/{id?}",
    defaults: new { controller = "Servicios" });

app.Run();
