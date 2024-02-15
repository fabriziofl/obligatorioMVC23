using Logica_De_Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_De_Negocio
{
    public class Post : Publicacion, IValidate
    {
        private string _imagen;
        private bool _privado;
        private List<Comentario> _comentarios = new List<Comentario>();
        private bool _censurado;

        public Post(string titulo, string texto, Miembro miembro, string imagen, bool privado) : base(titulo, texto, miembro)
        {
            this._imagen = imagen;
            this._privado = privado;
            this._censurado = false;
        }

        public bool Privado { get { return _privado; } }

        public bool Censurado { get { return _censurado; } }

        public List<Comentario> Comentarios { get {  return _comentarios; } }

        public void AgregarComentario(Comentario comentario)
        {
            _comentarios.Add(comentario);
        }

        public void CambiarEstado(bool estado)
        {
            _censurado = estado;
        }

        public override int CalcularVA()
        {
            int valorVA = base.CalcularVA();

            if (!_privado) { valorVA = valorVA +  10; }

            return valorVA;
        }

        public override string ToString()
        {
            string texto = Texto;

            if(texto.Length > 50) texto = Texto.Substring(0,50) + "...";

            return $" Id: {Id}\n Fecha: {DateTime}\n Titulo: {Titulo}\n Texto: {texto}\n";
        }

        public void ValidarDatos()
        {
            base.ValidarDatos();

            if(_imagen.Trim().Length == 0)
            {
                throw new Exception("La Imagen No Puede Estar Vacia");
            }

            string extension = _imagen.Substring(_imagen.LastIndexOf("."));

            if (extension != ".jpg" && extension != ".png")
            {
                throw new Exception("La Extension Debe Ser .jpg o .png");
            }
        }
    }
}
