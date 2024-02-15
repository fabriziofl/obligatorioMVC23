using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica_De_Negocio.Interfaces;

namespace Logica_De_Negocio
{
    public abstract class Usuario : IValidate, IEquatable<Usuario>
    {
        private string _email;
        private string _contrasenia;

        public Usuario(string email, string contrasenia) 
        { 
            this._email = email;    
            this._contrasenia = contrasenia;
        }

        public string Email
        {
            get { return _email; } 
        }

        public string Contrasenia
        {
            get { return _contrasenia; }
        }

        public bool Equals(Usuario? other)
        {
            return _email.Trim().ToLower() == other._email.Trim().ToLower();
        }

        public abstract void ValidarDatos();

        public abstract string Rol();
    }
}
