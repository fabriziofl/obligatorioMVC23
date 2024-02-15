using Logica_De_Negocio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Obligatorio_2.Controllers
{
    public class MiembroController : Controller
    {
        private Sistema _miSistema = Sistema.Instancia;

        public IActionResult VerPost()
        {
            if(!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            ViewBag.Posts = _miSistema.DevolverPostParaUsuario(email);

            return View();
        }

        public IActionResult EnviarSolicitud()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            ViewBag.Usuarios = _miSistema.DevolverMiembrosNoAmigos(email);

            return View();
        }

        [HttpPost]
        public IActionResult EnviarSolicitud(string email)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string emailSolicitante = HttpContext.Session.GetString("email");
            
            Miembro miembroSolicitante = (Miembro)_miSistema.BuscarUsuario(emailSolicitante);
            Miembro miembroSolicitado = (Miembro)_miSistema.BuscarUsuario(email);


            if(!miembroSolicitado.ExisteAmistad(miembroSolicitante) && !miembroSolicitado.ExisteSolicitud(miembroSolicitante))
            {
                try
                {
                    _miSistema.GenerarSolicitud(miembroSolicitante, miembroSolicitado);

                    ViewBag.Usuarios = _miSistema.DevolverMiembrosNoAmigos(email);

                    ViewBag.Message = "Solicitud Realizada con Exito";

                    return View();

                } catch(Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;

                    return View();
                }
            }

            ViewBag.Usuarios = _miSistema.DevolverMiembrosNoAmigos(email);

            ViewBag.ErrorMessage = "Existe Solicitud";

            return View();
        }

        public IActionResult VerSolicitudes()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            Miembro miembro = (Miembro)_miSistema.BuscarUsuario(email);

            ViewBag.Solicitudes = _miSistema.DevolverSolicitudesPendientes(miembro);

            return View();
        }

        [HttpPost]
        public IActionResult VerSolicitudes(int id, string estado)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            Miembro miembro = (Miembro)_miSistema.BuscarUsuario(email);

            _miSistema.AceptarSolicitud(miembro, id, estado);

            if (estado == "APROBADA")
            {
                _miSistema.AceptarSolicitud(miembro, id, estado);
                ViewBag.Message = "Solicitud Aceptada";
            }
            if (estado == "RECHAZADA")
            {
                _miSistema.RechazarSolicitud(miembro, id, estado);
                ViewBag.ErrorMessage = "Solicitud Rechazada";
            }

            ViewBag.Solicitudes = _miSistema.DevolverSolicitudesPendientes(miembro);

            return View();
        }

        public IActionResult RealizarPost()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult RealizarPost(string titulo, string texto, string imagen, bool privado)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(texto) || string.IsNullOrEmpty(imagen))
            {
                ViewBag.ErrorMessage = "Datos Ingresados no son Correctos";

                return View();
            }

            string email = HttpContext.Session.GetString("email");

            Miembro miembro = (Miembro)_miSistema.BuscarUsuario(email);

            try
            {
                _miSistema.CrearPost(titulo, texto, imagen, miembro, privado);

                ViewBag.Message = "Post Realizado Con Exito";
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;  
            }

            return View();
        }

        public IActionResult RealizarComentario()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            ViewBag.Posts = _miSistema.DevolverPostParaUsuario(email);

            return View();
        }

        [HttpPost]
        public IActionResult RealizarComentario(int id, string titulo, string comentario)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");
            
            string email = HttpContext.Session.GetString("email");

            Miembro miembro = (Miembro)_miSistema.BuscarUsuario(email);

            ViewBag.Posts = _miSistema.DevolverPostParaUsuario(email);

            if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(comentario))
            {
                ViewBag.ErrorMessage = "Datos Ingresados no son Correctos";

                return View();
            }

            try
            {
                _miSistema.ComentarPost(id, titulo, comentario, miembro);

                ViewBag.Message = "Comentario Realizado Con Exito";

            } catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
        }

        public IActionResult Reaccionar()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            ViewBag.Posts = _miSistema.DevolverPostParaUsuario(email);

            return View();
        }

        [HttpPost]
        public IActionResult Reaccionar(int id, string reaccion)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            ViewBag.Posts = _miSistema.DevolverPostParaUsuario(email);

            Miembro miembro = (Miembro)_miSistema.BuscarUsuario(email);

            bool reaccionBool = false;

            if(reaccion == "TRUE") reaccionBool = true;

            try
            {
                _miSistema.ReaccionarPublicacion(id, reaccionBool, miembro);

                ViewBag.Message = "Reaccion Realizada con Exito";
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
        }

        public IActionResult ListarPorTextoyNumero()
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            ViewBag.Posts = new List<Post>();

            return View();
        }

        [HttpPost]
        public IActionResult ListarPorTextoyNumero(int valor, string texto)
        {
            if (!ChequearRole()) return RedirectToAction("Error404", "Home");

            string email = HttpContext.Session.GetString("email");

            if (valor == null || string.IsNullOrEmpty(texto))
            {
                ViewBag.ErrorMessage = "Datos Ingresados Incorrectos";

                ViewBag.Posts = new List<Post>();

                return View();
            }

            try
            {
                ViewBag.Posts = _miSistema.DevolverPubliPorVAyTexto(texto, valor, email);

                return View();
            }
            catch(Exception ex) { 
                ViewBag.ErrorMessage = ex.Message;

                ViewBag.Posts = new List<Post>();

                return View();
            }
        }

        private bool ChequearRole()
        {
            bool esMiembro = false;

            string rol = HttpContext.Session.GetString("rol");

            if (rol == "MIEMBRO") esMiembro = true;

            return esMiembro;
        }
    }
}
