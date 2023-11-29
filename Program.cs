using Microsoft.AspNetCore.Authentication.Cookies;
using Plantilla_Agenda.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new ContextoDB(builder.Configuration.GetConnectionString("Conexiondb")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Persona/Login";
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Details",
    pattern: "Agendamiento/Details/{id}",
    defaults: new { controller = "Agendamiento", action = "Details" });

app.MapControllerRoute(
    name: "Create",
    pattern: "Agendamiento/Create",
    defaults: new { controller = "Agendamiento", action = "Create" });

app.MapControllerRoute(
    name: "Edit",
    pattern: "Agendamiento/Edit/{id}",
    defaults: new { controller = "Agendamiento", action = "Edit" });

app.MapControllerRoute(
    name: "List",
    pattern: "Agendamiento/List",
    defaults: new { controller = "Agendamiento", action = "List" });

app.MapControllerRoute(
    name: "Delete",
    pattern: "Agendamiento/Delete/{id}",
    defaults: new { controller = "Agendamiento", action = "Delete" });

app.MapControllerRoute(
    name: "Index",
    pattern: "Agendamiento",
    defaults: new { controller = "Agendamiento", action = "Index" });

app.MapControllerRoute(
    name: "Details",
    pattern: "Agenda/Details/{id}",
    defaults: new { controller = "Agenda", action = "Details" });

app.MapControllerRoute(
    name: "Create",
    pattern: "Agenda/Create",
    defaults: new { controller = "Agenda", action = "Create" });

app.MapControllerRoute(
    name: "Edit",
    pattern: "Agenda/Edit/{id}",
    defaults: new { controller = "Agenda", action = "Edit" });

app.MapControllerRoute(
    name: "List",
    pattern: "Agenda/List",
    defaults: new { controller = "Agenda", action = "List" });

app.MapControllerRoute(
    name: "Delete",
    pattern: "Agenda/Delete/{id}",
    defaults: new { controller = "Agenda", action = "Delete" });

app.MapControllerRoute(
    name: "Index",
    pattern: "Agenda",
    defaults: new { controller = "Agenda", action = "Index" });

app.MapControllerRoute(
    name: "Create",
    pattern: "Servicios",
    defaults: new { controller = "Servicios", action = "Create" });



app.MapControllerRoute(
    name: "Delete",
    pattern: "Servicios",
    defaults: new { controller = "Servicios", action = "Delete" });

app.Run();
