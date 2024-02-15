using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_De_Negocio
{
    public class Solicitud
    {
        private int _id;
        private Miembro _solicitante;
        private Miembro _solicitado;
        private EstadoDeSolicitud _estadoSolicitud;
        private DateTime _fechaSolicitud;
        
        public Solicitud(Miembro solicitante, Miembro solicitado, EstadoDeSolicitud estadoSolicitud) 
        {
            this._id = new Random().Next(1,1000);
            this._solicitante = solicitante;
            this._solicitado = solicitado;
            this._estadoSolicitud = estadoSolicitud;    
            this._fechaSolicitud = DateTime.Now;
        }

        public int Id { get { return _id; } } 

        public Miembro Solicitante { get { return _solicitante; } }

        public Miembro Solicitado { get { return _solicitado; } }

        public EstadoDeSolicitud EstadoDeSolicitud { get { return _estadoSolicitud; } }

        public void CambiarEstado(EstadoDeSolicitud estado)
        {
            this._estadoSolicitud = estado;
        }

        public override string ToString()
        {
            return $"Solicitud Id: " + _id;
        }
    }
}
