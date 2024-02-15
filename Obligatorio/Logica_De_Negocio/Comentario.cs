using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Logica_De_Negocio
{
    public class Comentario : Publicacion
    {
        private Post _post;
        private bool _privado;

    public Comentario(string titulo, string texto, Miembro autor, Post post) : base(titulo, texto, autor) 
    { 
        this._post = post;  
        this._privado = post.Privado;
    }

    public Post Post 
    { 
        get { return this._post; }
    }

    public override string ToString()
        {
            string datos = $" Tipo: Comentario\n Post_Id: {_post.Id}\n\n -- Lista de Reaciones --\n";

            foreach (Reaccion reaccion in Reacciones)
            {
                datos += $"\n -{reaccion}\n";
            }

            return datos;
        }
    }
}
