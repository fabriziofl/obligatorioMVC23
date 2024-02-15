using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_De_Negocio
{
    public class Administrador : Usuario
    {
        public Administrador(string email, string contrasenia) : base(email, contrasenia) { }
        
        public void BloquearMiembro(Miembro miembro){}

        public void CensurarPost(Post post){}

        public override string ToString()
        {
            return $" Tipo: Administrador\n email: {Email}";
        }

        public override void ValidarDatos()
        {
            if(Email.Trim().Length == 0)
            {
                throw new Exception("El Email No Puede Estar Vacio");
            }

            if (Contrasenia.Trim().Length == 0)
            {
                throw new Exception("La Contraseña No Puede Estar Vacia");
            }
        }

        public override string Rol()
        {
            return "ADMIN";
        }

    }
}
