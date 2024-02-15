using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_De_Negocio
{
    public class Miembro : Usuario, IComparable<Miembro>
    {
        private string _nombre;
        private string _apellido;
        private DateTime _fechaNac;
        private bool _bloqueado;
        private List<Miembro> _amigos = new List<Miembro>();
        private List<Solicitud> _solicitudes = new List<Solicitud>();
        private List<Publicacion> _publicaciones = new List<Publicacion>();

        public Miembro(string email, string contrasenia,string nombre, string apellido, DateTime fechaNac) : base (email, contrasenia)
        {   
            this._nombre = nombre;
            this._apellido = apellido;
            this._fechaNac = fechaNac;
            this._bloqueado = false;
        }

        public string Nombre
        {
            get { return _nombre; }
        }

        public string Apellido
        {
            get { return _apellido; }
        }

        public DateTime FechaNac
        {
            get { return _fechaNac; }
        }

        public bool Bloqueado
        {
            get { return _bloqueado; }
        }

        public List<Miembro> Amigos
        {
            get { return _amigos; }
        }

        public List<Solicitud> Solicitudes 
        {
            get { return _solicitudes; }
        }

        public List<Publicacion> Publicaciones
        {
            get { return _publicaciones; }
        }

        public void AgregarSolicitud(Solicitud solicitud)
        {
            _solicitudes.Add(solicitud);
        }

        public void ResponderSolicitud(Solicitud solicitud, EstadoDeSolicitud nuevoEstado)
        {
            solicitud.CambiarEstado(nuevoEstado);

            if(nuevoEstado == EstadoDeSolicitud.APROBADA)
            {
                AgregarAmigo(solicitud.Solicitante);
            }
        }

        public void AgregarPublicacion(Publicacion nuevaPublicacion)
        {
            _publicaciones.Add(nuevaPublicacion);
        }

        public void RealizarComentario(string titulo, string texto, Post post)
        {
            Publicacion nuevaPublicacion = new Comentario(titulo, texto, this, post);

            _publicaciones.Add(nuevaPublicacion);
        }

        public void AgregarAmigo(Miembro nuevoAmigo)
        {
            if (!ExisteAmistad(nuevoAmigo))
            {
                _amigos.Add(nuevoAmigo);
            }
            
        }

        public bool ExisteAmistad(Miembro nuevoAmigo)
        {
            bool existeAmistad = false;

            foreach (Miembro amigo in _amigos)
            {
                if(amigo == nuevoAmigo)
                {
                    existeAmistad = true;
                }
            }

            return existeAmistad;
        }

        public bool ExisteSolicitud(Miembro miembroSolicitado)
        {
            bool existeSolicitud = false;

            foreach (Solicitud solicitud in _solicitudes)
            {
                if (solicitud.Solicitante == miembroSolicitado)
                {
                    existeSolicitud = true;
                }
            }

            return existeSolicitud;
        }

        public void CambiarEstado(bool estado)
        {
            _bloqueado = estado;
        }

        public override string ToString()
        {
            string datos = $" Tipo: Miembro\n Email: {Email}\n Nombre: {_nombre}\n Apellido: {_apellido}\n Fecha de Nacimiento: {_fechaNac}\n\n Lista de Amigos:\n";

            foreach (Miembro amigo in _amigos)
            {
                datos += $"\n {amigo._nombre} \n";
            }

            datos += "\n Solicitudes de Amistad: \n";

            foreach (Solicitud solicitud in _solicitudes)
            {
                datos += $"\n {solicitud} \n";
            }

            return datos;
        }

        public override void ValidarDatos()
        {
            if (Email.Trim().Length == 0)
            {
                throw new Exception("El Email No Puede Estar Vacio");
            }

            if (Contrasenia.Trim().Length == 0)
            {
                throw new Exception("La Contraseña No Puede Estar Vacia");
            }

            if (_nombre.Trim().Length == 0)
            {
                throw new Exception("El Nombre No Puede Estar Vacio");
            }

            if (_apellido.Trim().Length == 0)
            {
                throw new Exception("El Apellido No Puede Estar Vacio");
            }

            if (_fechaNac.Year > DateTime.Now.Year)
            {
                throw new Exception("La Fecha de Nacimiento es Incorrecto");
            }
        }

        public int CompareTo(Miembro? other)
        {
            int resultado = _apellido.CompareTo(other._apellido);
            
            if(resultado == 0 )
            {
                resultado = _nombre.CompareTo(other._nombre);
            }

            return resultado;
        }

        public override string Rol()
        {
            return "MIEMBRO";
        }
    }
}
