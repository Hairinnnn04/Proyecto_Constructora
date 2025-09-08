using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miprimerproyecto.Models;

namespace miprimerproyecto.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public IActionResult EliminarUsuario(int id, [FromServices] ConstructoraDbContext db)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                return Unauthorized();
            }
            var usuario = db.Usuarios.Find(id);
            if (usuario != null)
            {
                db.Usuarios.Remove(usuario);
                db.SaveChanges();
            }
            return RedirectToAction("Usuarios");
        }
        public IActionResult Presupuesto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerarPresupuesto(string prompt)
        {
            const string GeminiApiKey = "AIzaSyAJmbunmglS2y3BTeyMhalSxL-ULuyJiRc";
            const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1/models/gemini-1.5-pro-002:generateContent?key=";
            using (var httpClient = new HttpClient())
            {
                var promptFinal = "Eres un experto en construcción. Da recomendaciones detalladas y prácticas para el siguiente caso: " + prompt;
                var requestBody = new
                {
                    contents = new object[] {
                        new {
                            parts = new object[] {
                                new { text = promptFinal }
                            }
                        }
                    }
                };
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var response = await httpClient.PostAsync(
                    GeminiApiUrl + GeminiApiKey,
                    new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json")
                );
                var result = await response.Content.ReadAsStringAsync();
                var obj = Newtonsoft.Json.Linq.JObject.Parse(result);
                var texto = obj["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
                if (string.IsNullOrEmpty(texto))
                {
                    texto = "No se pudo generar la recomendación.<br><br><strong>Respuesta completa de la API:</strong><br><pre>" + result + "</pre>";
                }
                return Json(new { recomendacion = texto });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarModelosGemini()
        {
            const string GeminiApiKey = "AIzaSyAJmbunmglS2y3BTeyMhalSxL-ULuyJiRc";
            const string GeminiListModelsUrl = "https://generativelanguage.googleapis.com/v1/models?key=";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(GeminiListModelsUrl + GeminiApiKey);
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
        }
        public IActionResult Dashboard()
    {
        var usuario = HttpContext.Session.GetString("Usuario");
        var rol = HttpContext.Session.GetString("Rol");
        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(rol))
        {
            return RedirectToAction("Login", "Account");
        }
        ViewBag.Usuario = usuario;
        ViewBag.Rol = rol;
        // Solo administrador y vendedor pueden acceder al dashboard completo
        if (rol == "Cliente")
        {
            return RedirectToAction("Index");
        }
        return View();
    }
    public IActionResult Proyectos([FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol == "Cliente")
        {
            ViewBag.Rol = rol;
            // Los clientes solo pueden ver proyectos, no registrar ni editar
            var proyectos = db.Proyectos.ToList();
            ViewBag.Proyectos = proyectos;
            return View("Proyectos");
        }
        else if (rol == "Administrador" || rol == "Vendedor")
        {
            var proyectos = db.Proyectos.ToList();
            var materiales = db.Materiales.ToList();
            var detalles = db.DetalleProyectos.ToList();
            ViewBag.Proyectos = proyectos;
            ViewBag.Materiales = materiales;
            ViewBag.Detalles = detalles;
            ViewBag.Rol = rol;
            return View("Proyectos");
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }
    }

    [HttpPost]
    public IActionResult RegistrarProyecto(int[] MaterialId, int[] Cantidad, [FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol != "Administrador" && rol != "Vendedor")
        {
            return RedirectToAction("Proyectos");
        }
        var usuarioNombre = HttpContext.Session.GetString("Usuario");
        var usuario = db.Usuarios.FirstOrDefault(u => u.Nombre == usuarioNombre);
        if (usuario == null) return RedirectToAction("Ventas");

        decimal total = 0;
        for (int i = 0; i < MaterialId.Length; i++)
        {
            var mat = db.Materiales.Find(MaterialId[i]);
            if (mat != null)
            {
                total += mat.Precio * Cantidad[i];
            }
        }

        var proyecto = new Proyecto
        {
            UsuarioId = usuario.Id,
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddMonths(6),
            Presupuesto = total
        };
        db.Proyectos.Add(proyecto);
        db.SaveChanges();

        for (int i = 0; i < MaterialId.Length; i++)
        {
            var mat = db.Materiales.Find(MaterialId[i]);
            if (mat != null)
            {
                var detalle = new DetalleProyecto
                {
                    ProyectoId = proyecto.Id,
                    MaterialId = mat.Id,
                    Cantidad = Cantidad[i],
                    CostoUnitario = mat.Precio
                };
                db.DetalleProyectos.Add(detalle);
                // Actualizar stock
                mat.Stock -= Cantidad[i];
            }
        }
        db.SaveChanges();
        return RedirectToAction("Proyectos");
    }

    public IActionResult Materiales([FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol == "Cliente")
        {
            ViewBag.Rol = rol;
            // Los clientes solo pueden ver materiales
            var materiales = db.Materiales.ToList();
            ViewBag.Materiales = materiales;
            return View();
        }
        else if (rol == "Administrador" || rol == "Vendedor")
        {
            var materiales = db.Materiales.ToList();
            ViewBag.Materiales = materiales;
            ViewBag.Rol = rol;
            return View();
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }
    }

    [HttpPost]
    public IActionResult AgregarMaterial(Material material, [FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol != "Administrador")
        {
            return RedirectToAction("Materiales");
        }
        db.Materiales.Add(material);
        db.SaveChanges();
        return RedirectToAction("Materiales");
    }

    [HttpPost]
    public IActionResult EditarMaterial(Material material, [FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol != "Administrador")
        {
            return RedirectToAction("Materiales");
        }
        var m = db.Materiales.Find(material.Id);
        if (m != null)
        {
            m.Nombre = material.Nombre;
            m.Descripcion = material.Descripcion;
            m.Precio = material.Precio;
            m.Stock = material.Stock;
            db.SaveChanges();
        }
        return RedirectToAction("Materiales");
    }
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Usuarios([FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol != "Administrador")
        {
            ViewBag.Rol = rol;
            return View();
        }
        var usuarios = db.Usuarios.ToList();
        var roles = db.Roles.ToList();
        ViewBag.Usuarios = usuarios;
        ViewBag.Roles = roles;
        ViewBag.Rol = rol;
        return View();
    }

    [HttpPost]
    public IActionResult AgregarUsuario(Usuario usuario, [FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol != "Administrador")
            return RedirectToAction("Dashboard");

        // Guardar contraseña (sin cifrado, para ejemplo; en producción usar hash)
        db.Usuarios.Add(usuario);
        db.SaveChanges();
        return RedirectToAction("Usuarios");
    }

    [HttpPost]
    public IActionResult EditarUsuario(Usuario usuario, [FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol != "Administrador")
            return RedirectToAction("Dashboard");

        var u = db.Usuarios.Find(usuario.Id);
        if (u != null)
        {
            u.Nombre = usuario.Nombre;
            u.Email = usuario.Email;
            u.RolId = usuario.RolId;
            if (!string.IsNullOrEmpty(usuario.Password))
            {
                u.Password = usuario.Password;
            }
            db.SaveChanges();
        }
        return RedirectToAction("Usuarios");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
    
}
