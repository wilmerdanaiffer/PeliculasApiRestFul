namespace PeliculasApiRestFul.Entidades
{
    public class PeliculasGeneros
    {
        public int PeliculaId { get; set; }
        public int GeneroId { get; set; }
        public Genero? genero { get; set; }
        public Pelicula? pelicula { get; set; }

    }
}
