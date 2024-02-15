using Logica_De_Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Obligatorio_2.Controllers
{
    public class AdminController : Controller
    {
        private Sistema _miSistema = Sistema.Instancia;

        public IActionResult ListarUsuarios()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            ViewBag.Usuarios = _miSistema.DevolverMiembros();

            return View();
        }

        public IActionResult BloquearUsuario()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            ViewBag.Usuarios = _miSistema.DevolverMiembros();

            return View();
        }

        [HttpPost]
        public IActionResult BloquearUsuario(string email)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            if (_miSistema.BuscarUsuario(email) != null && _miSistema.BuscarUsuario(email) is Miembro)
            {
                Miembro usuario = (Miembro)_miSistema.BuscarUsuario(email);

                usuario.CambiarEstado(true);

                ViewBag.Message = "Usuario Bloqueado";
                ViewBag.Usuarios = _miSistema.DevolverMiembros();

                return View();

            }

            ViewBag.Usuarios = _miSistema.DevolverMiembros();
            return View();
        }

        public IActionResult BloquearPost()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            ViewBag.Posts = _miSistema.DevolverTodosLosPostNoBaneados();

            return View();
        }

        [HttpPost]
        public IActionResult BloquearPost(int id)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            if (_miSistema.BuscarPost(id) != null && _miSistema.BuscarPost(id) is Post)
            {
                Post post = (Post)_miSistema.BuscarPost(id);

                post.CambiarEstado(true);
                ViewBag.Posts = _miSistema.DevolverTodosLosPostNoBaneados();
                ViewBag.Message = "Post Bloqueado";

                return View();

            }

            ViewBag.Posts = _miSistema.DevolverTodosLosPostNoBaneados();
            ViewBag.ErrorMessage = "Post No Existe";

            return View();
        }

        private bool ChequearRole()
        {
            bool esMiembro = false;

            string rol = HttpContext.Session.GetString("rol");

            if (rol == "ADMIN") esMiembro = true;

            return esMiembro;
        }

    }
}
