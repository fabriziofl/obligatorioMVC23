using Logica_De_Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Obligatorio_2.Controllers
{
    public class UsuarioController : Controller
    {
        private Sistema _miSistema = Sistema.Instancia;

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            if(_miSistema.BuscarUsuario(email) != null)
            {
                Usuario usuario = _miSistema.BuscarUsuario(email);

                if (usuario.Contrasenia == contrasenia)
                {

                    HttpContext.Session.SetString("rol", usuario.Rol());
                    HttpContext.Session.SetString("email", email);

                    if(usuario is Miembro miembro)
                    {
                        if(miembro.Bloqueado) HttpContext.Session.SetString("rol", "MIEMBRO_BLOQUEADO");
                    }
                        
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.ErrorMessage = "Los Datos Ingresados son Incorrectos";

            return View();
            
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(string nombre, string apellido, DateTime fechaNacimiento, string email, string contrasenia)
        {

            if(string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasenia) || fechaNacimiento > DateTime.Now)
            {
                ViewBag.ErrorMessage = "Los Datos Ingresados son Incorrectos";
                
                return View();
            }else if(_miSistema.UsuarioExiste(email)){
                ViewBag.ErrorMessage = "Este Usuario Ya Existe";

                return View();
            }
            else{
                try
                {
                    Miembro miembroNuevo = new Miembro(email, contrasenia, nombre, apellido, fechaNacimiento);

                    _miSistema.AltaMiembro(miembroNuevo);

                    HttpContext.Session.SetString("rol", "MIEMBRO");
                    HttpContext.Session.SetString("email", email);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
