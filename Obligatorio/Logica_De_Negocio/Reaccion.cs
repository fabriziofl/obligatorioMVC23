using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_De_Negocio
{
    public class Reaccion
    {
        private bool _meGusta;
        private Miembro _miembro;

        public Reaccion(bool meGusta, Miembro miembro)
        {
            this._meGusta = meGusta;    
            this._miembro = miembro;
        }

        public bool MeGusta
        {
            get { return _meGusta; }
        }

        public override string ToString()
        {
            return $" Reaccion\n Like: {_meGusta}\n Miembro: \n {_miembro.Nombre}";
        }
    }
}
