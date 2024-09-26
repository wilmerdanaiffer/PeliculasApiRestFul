namespace PeliculasApiRestFul.Entidades
{
    public class PeliculasActores
    {
        public int PeliculaId { get; set; }
        public int ActorId { get; set; }
        public string Personaje { get; set; }
        public int Orden { get; set; }
        public Pelicula pelicula { get; set; }
        public Actor actor { get; set; }
    }
}
