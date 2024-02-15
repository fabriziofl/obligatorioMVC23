using Logica_De_Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_De_Negocio
{
    public abstract class Publicacion : IValidate, IComparable<Publicacion>
    {
        private int _id;
        private static int s_ultimoId;
        private string _titulo;
        private string _texto;
        private DateTime _fecha;
        private Miembro _autor;
        private int _valorDeAceptaciones;
        private List<Reaccion> _reacciones = new List<Reaccion>();

        public Publicacion(string titulo, string texto, Miembro miembro)
        {
            s_ultimoId++;
            this._id = s_ultimoId;
            this._titulo = titulo;
            this._texto = texto;
            this._autor = miembro;
            this._fecha = DateTime.Now;
        }

        public int Id { get { return _id; } }

        public DateTime DateTime { get { return _fecha; } }

        public string Titulo { get { return _titulo; } }

        public string Texto { get { return _texto; } }

        public Miembro Autor { get { return _autor; } }

        public List<Reaccion> Reacciones
        {
            get { return _reacciones; }
        }

        public void AgregarReaccion(Reaccion reaccion)
        {
            _reacciones.Add(reaccion);
        }

        public virtual int CantLike()
        {
            int cantLike = 0;

            foreach (Reaccion reaccion in _reacciones)
            {
                if(reaccion.MeGusta) { cantLike++; }
            }

            return cantLike;
        }

        public virtual int CantDisLike()
        {
            int cantDisLike = 0;

            foreach (Reaccion reaccion in _reacciones)
            {
                if (!reaccion.MeGusta) { cantDisLike++; }
            }

            return cantDisLike;
        }

        public virtual int CalcularVA()
        { 
            return (CantLike() * 5) + (CantDisLike() * (-2));
        }

        public abstract override string ToString();

        public void ValidarDatos()
        {
            if (_texto.Trim().Length ==0)
            {
                throw new Exception("El Texto No Puedo Estar Vacio");
            }

            if (_titulo.Trim().Length < 3)
            {
                throw new Exception("El Titulo debe Contener al Menos 3 Caracteres");
            }
        }

        public int CompareTo(Publicacion? other)
        {
            return _fecha.CompareTo(other._fecha) * -1;
        }
    }
}
