using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Logica_De_Negocio
{
    public class Sistema
    {
        private List<Usuario> _usuarios = new List<Usuario>();
        private List<Publicacion> _publicaciones = new List<Publicacion>();
        private Random _random = new Random();
        private static Sistema _instancia;

        public Sistema() 
        {
            PrecargaMiembros();
            PrecargaAdmin();
            PrecargaSolicitudes();
            AgregarVinculos();
            PrecargaPublicaciones();
            PrecargaReacciones();
        }

        public static Sistema Instancia
        {
            get
            {
                if (_instancia == null) _instancia = new Sistema();
                return _instancia;
            }
        }

        public void AltaMiembro(Miembro miembro)
        {
            miembro.ValidarDatos();

            if (!_usuarios.Contains(miembro))
            {
                _usuarios.Add(miembro);
            }
        }

        public void AltaAdmin(Administrador administrador)
        {
            administrador.ValidarDatos();

            if (!_usuarios.Contains(administrador))
            {
                _usuarios.Add(administrador);
            }
        }

        private void PrecargaMiembros()
        {
            for(int i = 0; i < 10; i++)
            {
                string email = "Miembro_" + i;
                string contrasenia = "Contra_" + i;
                string nombre = "Nombre_" + i;
                string apellido = "Apellido_" + i;
                DateTime fechaNac = DateTime.Now;

                Usuario miembroNuevo = new Miembro(email, contrasenia, nombre, apellido, fechaNac);
                AltaMiembro((Miembro)miembroNuevo);
            }
        }

        private void PrecargaAdmin()
        {
            Usuario adminNuevo = new Administrador("Admin", "Admin_1");

            AltaAdmin((Administrador)adminNuevo);
        }

        // Metodo para precarga de solicitudes. Para cada miembro de la lista le precarga una solicitud de amistad,
        // si la solicitud existe o la amistad existe la nueva solicitud NO se crea. A su vez si la solicitud se crea con el Estado Aprobado
        // se crea tambien la amistad reciproca.
        private void PrecargaSolicitudes()
        {
            for (int i = 0; i < _usuarios.Count; i++)
            {
                if (_usuarios[i] is Miembro)
                {
                    Miembro miembroSolicitante = (Miembro)_usuarios[i];
                    Miembro miembroSolicitado;

                    do
                    {
                        miembroSolicitado = ObtenerMiembroAleatorio();
                    } while (miembroSolicitante == miembroSolicitado);

                    

                    if(!miembroSolicitado.ExisteAmistad(miembroSolicitado) && !miembroSolicitado.ExisteSolicitud(miembroSolicitado))
                    {
                        EstadoDeSolicitud estadoRandom = EstadoDeSolicitud.PENDIENTE_APROBACION;

                        Solicitud nuevaSolicitud = new Solicitud(miembroSolicitante, miembroSolicitado, estadoRandom);

                        miembroSolicitado.AgregarSolicitud(nuevaSolicitud);

                        if (estadoRandom == EstadoDeSolicitud.APROBADA)
                        {
                            miembroSolicitado.AgregarAmigo(miembroSolicitante);
                            miembroSolicitante.AgregarAmigo(miembroSolicitado);

                        }
                    }
                }
            };
        }

        //Metodo para generar al menos dos vinculos de amistad de forma reciproca
        private void AgregarVinculos()
        {
            for (int i = 0; i < 2; i++)
            {
                Miembro miembroUno = ObtenerMiembroAleatorio();
                Miembro miembroDos;

                do
                {
                    miembroDos = ObtenerMiembroAleatorio();
                } while (miembroUno == miembroDos);

                miembroUno.AgregarAmigo(miembroDos);
                miembroDos.AgregarAmigo(miembroUno);
            }
        }

        //Precarga 15 Publicaciones y luego Precarga 3 Comentarios a cada una
        private void PrecargaPublicaciones()
        {
            for (int i = 0; i < 15; i++)
            {
                string titulo = "Titulo_" + i;
                string texto = "Publicacion_" + i;
                string imagen = i + "_imagen.jpg";
                bool censurado  = _random.Next(2) == 1;
                Miembro miembro = ObtenerMiembroAleatorio();

                Publicacion nuevaPublicacion = new Post(titulo, texto, miembro, imagen, censurado);

                Post nuevoPost = (Post)nuevaPublicacion;
                
                nuevoPost.ValidarDatos();

                _publicaciones.Add(nuevaPublicacion);
                
                miembro.AgregarPublicacion(nuevaPublicacion);  
            }

            AgregarComentarios();
        }

        private void AgregarComentarios()
        {
            List<Publicacion> nuevosComentarios = new List<Publicacion>();

            foreach (Publicacion post in _publicaciones)
            {
                if(post is Post)
                {
                    for(int i = 0;i < 3; i++)
                    {
                        string titulo = "Titulo_Comentario_" + i;
                        string texto = "Comentario_" + i;
                        Post post2 = (Post)post;
                        Miembro miembro = ObtenerMiembroAleatorio();
                        

                        Publicacion nuevoComentario = new Comentario(titulo, texto, miembro, post2);

                        nuevoComentario.ValidarDatos();
                        nuevosComentarios.Add(nuevoComentario);

                        miembro.AgregarPublicacion(nuevoComentario);
                        post2.AgregarComentario((Comentario)nuevoComentario);
                    }
                }
            }

            _publicaciones.AddRange(nuevosComentarios);
        }

        //Precarga de Reacciones para Post y Comentarios
        private void PrecargaReacciones()
        {
            List<Post> posts = FiltrarPost();
            List<Comentario> comentarios = FiltrarComentarios();

            for (int i = 0; i < 20; i++)
            {
                int numRan = _random.Next(0, posts.Count);
                Reaccion nuevaReaccion = GenerarReaccion();

                posts[numRan].AgregarReaccion(nuevaReaccion);
            }

            for (int i = 0; i < 20; i++)
            {
                int numRan = _random.Next(0, comentarios.Count);
                Reaccion nuevaReaccion = GenerarReaccion();

                comentarios[numRan].AgregarReaccion(nuevaReaccion);
            }

        }

        private Miembro ObtenerMiembroAleatorio()
        {
            Miembro miembro = null;

            for (int i = 0; i < _usuarios.Count; i++)
            {
                int numRan = _random.Next(_usuarios.Count);

                if (_usuarios[numRan] is Miembro)
                {
                    miembro = (Miembro)_usuarios[numRan];
                    break; 
                }
            }

            return miembro;
        }

        private List<Post> FiltrarPost()
        {
            List<Post> postList = new List<Post>();

            foreach (Publicacion post in _publicaciones)
            {
                if(post is Post) { postList.Add((Post)post); }
            }

            return postList;
        }

        private List<Comentario> FiltrarComentarios()
        {
            List<Comentario> comentList = new List<Comentario>();

            foreach (Publicacion coment in _publicaciones)
            {
                if (coment is Comentario) { comentList.Add((Comentario)coment); }
            }

            return comentList;
        }

        private Reaccion GenerarReaccion()
        {
            bool meGusta = _random.Next(2) == 0;
            Miembro miembro = ObtenerMiembroAleatorio();

            Reaccion nuevaReaccion = new Reaccion(meGusta, miembro);

            return nuevaReaccion;
        }

        public bool UsuarioExiste(string emailMiembro)
        {
            bool existe = false;

            if (BuscarUsuario(emailMiembro) != null)
            {
                existe = true;
            }

            return existe;
        }

        public Usuario? BuscarUsuario(string emailMiembro)
        {
            Usuario? usuarioBuscado = null;    

            foreach (Usuario usuario in _usuarios)
            {
                if (usuario.Email == emailMiembro)
                {
                    usuarioBuscado = usuario;
                }
            }

            return usuarioBuscado;
        }

        public Publicacion BuscarPost(int id)
        {
            Publicacion postBuscado = null;

            foreach (Publicacion publicacion in _publicaciones)
            {
                if (publicacion.Id == id)
                {
                    postBuscado = publicacion;
                }
            }

            return postBuscado;
        }

        //Metodo creado para devolver todos los post que contiene la lista publicacion no baneados
        public List<Post> DevolverTodosLosPostNoBaneados()
        {
            List<Post> listaDePost = new List<Post>();

            foreach (Publicacion publicacion in _publicaciones)
            {
                if(publicacion is Post post && !post.Censurado) listaDePost.Add(post);
            }

            return listaDePost;
        }

        public List<Miembro> DevolverMiembros()
        {
            List<Miembro> listaMiembros = new List<Miembro>();

            foreach (Usuario usuario in _usuarios)
            {
                if(usuario is Miembro miembro && !miembro.Bloqueado)
                {
                    listaMiembros.Add((Miembro)usuario);
                }
            }

            listaMiembros.Sort();

            return listaMiembros;
        }


        //Devuelvo los Miembros que no son mis amigos
        public List<Miembro> DevolverMiembrosNoAmigos(string email)
        {
            Miembro miembroSolicitante = (Miembro)BuscarUsuario(email);

            List<Miembro> listaMiembros = new List<Miembro>();

            foreach (Usuario usuario in _usuarios)
            {
                if (usuario is Miembro miembro && !miembro.Bloqueado && !miembroSolicitante.ExisteAmistad(miembro))
                {
                    listaMiembros.Add((Miembro)usuario);
                }
            }

            listaMiembros.Sort();

            return listaMiembros;
        }

        //Devuelve solamente los Post del Usuario
        public List<Post> DevolverPostParaUsuario(string emailUsuario)
        {
            List <Post> posts = new List<Post>();

            Miembro miembroUno = (Miembro)BuscarUsuario(emailUsuario);

            foreach (Publicacion publicacion in _publicaciones)
            {
                if (publicacion is Post post)
                {
                    if (post.Privado)
                    {
                        string emailAutor = post.Autor.Email;

                        Miembro autorPost = (Miembro)BuscarUsuario(emailAutor);

                        if (miembroUno.ExisteAmistad(autorPost))
                        {
                            posts.Add(post);
                        }
                    }
                    else
                    {
                        if (!post.Censurado)
                        {
                            posts.Add(post);
                        }
                    }    
                }     
            }
            return posts;
        }

        //Devuelve todas las publicaciones(Post y Comentario) del Usuario
        public List<Publicacion> DevolverPublicacionesParaUsuario(string emailUsuario)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();

            List<Post> postsUsuario = DevolverPostParaUsuario(emailUsuario);

            foreach (Post post in postsUsuario)
            {
                publicaciones.Add(post);

                foreach (Comentario comentario in post.Comentarios)
                {
                    publicaciones.Add(comentario);
                }
            }

            return publicaciones; 
        }

        public void GenerarSolicitud(Miembro solicitante, Miembro solicitado)
        {
            Solicitud nuevaSolicitud = new Solicitud(solicitante, solicitado, EstadoDeSolicitud.PENDIENTE_APROBACION);

            solicitado.AgregarSolicitud(nuevaSolicitud);
        }

        public List<Solicitud> DevolverSolicitudesPendientes(Miembro miembro)
        {
            List<Solicitud> solicitudesPendientes = new List<Solicitud>();

            foreach(Solicitud solicitud in miembro.Solicitudes)
            {
                if(solicitud.EstadoDeSolicitud == EstadoDeSolicitud.PENDIENTE_APROBACION)
                {
                    solicitudesPendientes.Add(solicitud);
                }
            }

            return solicitudesPendientes;
        }

        public void AceptarSolicitud(Miembro miembro, int id, string action)
        {
            Solicitud solicitud = BuscarSolicitud(miembro, id);

            solicitud.CambiarEstado(EstadoDeSolicitud.APROBADA);

            solicitud.Solicitante.AgregarAmigo(solicitud.Solicitado);
            solicitud.Solicitado.AgregarAmigo(solicitud.Solicitante);
        }

        public void RechazarSolicitud(Miembro miembro, int id, string action)
        {
            Solicitud solicitud = BuscarSolicitud(miembro, id);

            solicitud.CambiarEstado(EstadoDeSolicitud.RECHAZADA);
        }

        private Solicitud BuscarSolicitud(Miembro miembro, int id)
        {
            Solicitud? solicitudBuscada = null;

            foreach (Solicitud solicitud in miembro.Solicitudes)
            {
                if(solicitud.Id == id)
                {
                    solicitudBuscada = solicitud;
                }
            }

            return solicitudBuscada;
        }

        public void CrearPost(string titulo, string texto, string imagen, Miembro miembro, bool privado)
        {
            Post nuevoPost = new Post(titulo,texto,miembro,imagen, privado);

            nuevoPost.ValidarDatos();

            _publicaciones.Add(nuevoPost);
        }

        public void ComentarPost(int id, string titulo, string comentario, Miembro miembro)
        {
            Post post = (Post)BuscarPost(id);

            Comentario nuevoComentario = new Comentario(titulo,comentario,miembro, post);

            nuevoComentario.ValidarDatos();

            post.AgregarComentario(nuevoComentario);
        }

        public void ReaccionarPublicacion(int idPost, bool reaccion, Miembro miembro)
        {
            Reaccion nuevaReaccion = new Reaccion(reaccion, miembro);

            Publicacion publicacion = BuscarPost(idPost);

            publicacion.AgregarReaccion(nuevaReaccion);
        }

        public List<Publicacion> DevolverPubliPorVAyTexto(string texto, int valor, string emailUsuario)
        {
            List<Publicacion> publicacionesPorTextyVA = new List<Publicacion>();

            List<Publicacion> publicaciones = DevolverPublicacionesParaUsuario(emailUsuario);

            foreach (Publicacion publicacion in publicaciones)
            {
                if (publicacion.CalcularVA() > valor && publicacion.Titulo.ToLower().Contains(texto.ToLower())) { publicacionesPorTextyVA.Add(publicacion); }
            }

            return publicacionesPorTextyVA;
        }
    }
}
