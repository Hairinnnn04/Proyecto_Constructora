using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miprimerproyecto.Models;

namespace miprimerproyecto.Controllers
{
    public class HomeController : Controller
    {
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
            return View();
        }
    public IActionResult Proyectos([FromServices] ConstructoraDbContext db)
    {
        var rol = HttpContext.Session.GetString("Rol");
        if (rol != "Administrador" && rol != "Vendedor")
        {
            ViewBag.Rol = rol;
            return View();
        }
    var proyectos = db.Proyectos.ToList();
    var materiales = db.Materiales.ToList();
    var detalles = db.DetalleProyectos.ToList();
    ViewBag.Proyectos = proyectos;
    ViewBag.Materiales = materiales;
    ViewBag.Detalles = detalles;
        ViewBag.Rol = rol;
    return View("Proyectos");
    }

    [HttpPost]
    public IActionResult RegistrarProyecto(int[] MaterialId, int[] Cantidad, [FromServices] ConstructoraDbContext db)
    {
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
        if (rol != "Administrador" && rol != "Vendedor")
        {
            ViewBag.Rol = rol;
            return View();
        }
    var materiales = db.Materiales.ToList();
    ViewBag.Materiales = materiales;
        ViewBag.Rol = rol;
        return View();
    }

    [HttpPost]
    public IActionResult AgregarMaterial(Material material, [FromServices] ConstructoraDbContext db)
    {
        db.Materiales.Add(material);
        db.SaveChanges();
        return RedirectToAction("Materiales");
    }

    [HttpPost]
    public IActionResult EditarMaterial(Material material, [FromServices] ConstructoraDbContext db)
    {
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
