Arquitectura MVC: El proyecto sigue el patrón de arquitectura Modelo-Vista-Controlador (MVC), donde los modelos representan los datos, las vistas manejan la presentación y los controladores gestionan la lógica del negocio y la interacción entre modelos y vistas.

Autenticación y Autorización: Se utiliza la autenticación basada en cookies con el paquete Microsoft.AspNetCore.Authentication.Cookies. Además, hay decoradores de autorización como [Authorize] en algunos métodos del controlador para restringir el acceso a usuarios autenticados.

Acceso a Datos con Dapper y ADO.NET: La capa de acceso a datos utiliza Dapper, que es una biblioteca de mapeo objeto-relacional (ORM) para .NET, junto con ADO.NET puro para interactuar con una base de datos MySQL. Se realizan operaciones como obtener agendas, agendar y cancelar citas, etc.

Registro y Gestión de Usuarios: Hay funciones relacionadas con el registro y gestión de usuarios, con métodos para iniciar sesión, registrar administradores y otras operaciones relacionadas con la gestión de personas.

Vistas y ViewModels: Se utilizan vistas y ViewModels para representar la interfaz de usuario y los datos que se presentan en las vistas. Algunos ejemplos de ViewModels incluyen IndexViewModel y AgendaCreateViewModel.

Validación de Datos: Se utiliza el atributo [Required] del espacio de nombres System.ComponentModel.DataAnnotations para la validación de datos en algunos modelos.

Aplicación ASP.NET Core 6 MVC utilizando Dapper para administrar citas y usuarios en MySQL.Data 8.0.2, con énfasis en consultas puras y procedimientos almacenados mediante ADO.NET.

/AgendaEmpresa
  /Controllers
    - PersonaController.cs
    - OtroController.cs
    ... (otros controladores)
  /Models
    - Persona.cs
    - OtraEntidad.cs
    ... (otros modelos)
  /Repositories
    - AgendaRepository.cs
    - AgendamientoRepository.cs
    - PersonaRepository.cs
    - UsuarioRepository.cs
    - AgendaRepository.cs
    - AgendamientoRepository.cs
    - ServicioRepository.cs
    - 
  /Views
    /Agenda
    - Create.cshtml
      - Delete.cshtml
      - Details.cshtml
      - Edit.cshtml
      - Index.cshtml
      - List.cshtml
    /Agendamientos
      - Create.cshtml
      - Delete.cshtml
      - Details.cshtml
      - Edit.cshtml
      - Index.cshtml
      - List.cshtml
    /Sede
    - Create.cshtml
      - Delete.cshtml
      - Details.cshtml
      - Edit.cshtml
      - Index.cshtml
      - List.cshtml
   /Servicio
    - Create.cshtml
      - Delete.cshtml
      - Details.cshtml
      - Edit.cshtml
      - Index.cshtml
      - List.cshtml
    /Persona
      - Index.cshtml
      - Detalles.cshtml
 /Models
  - Agenda.cs
  - Agendamientos.cs
  - Asistencia.cs
  - Cancelacion.cs
  - Horario.cs
  - Permiso.cs
  - Persona.cs
  - Rol.cs
  - RolPermiso.cs
  - Sede.cs
  - Servicio.cs
  /wwwroot
    /css
      - site.css
      
    /js
      - site.js
      
  - appsettings.json
  - Program.cs
  - Startup.cs
