using Microsoft.AspNetCore.Mvc;
using miprimerproyecto.Models;

namespace miprimerproyecto.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
    public IActionResult Login(string email, string password, [FromServices] ConstructoraDbContext db)
        {
            var resultado = db.Usuarios
                .Where(u => u.Email == email)
                .Join(db.Roles, u => u.RolId, r => r.Id, (u, r) => new { Usuario = u, RolNombre = r.Nombre })
                .FirstOrDefault();

            if (resultado != null && password == resultado.Usuario.Password) // Aquí deberías usar hash
            {
                // Guardar usuario en sesión
                HttpContext.Session.SetString("Usuario", resultado.Usuario.Nombre);
                HttpContext.Session.SetString("Rol", resultado.RolNombre);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }
    }
}
