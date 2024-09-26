namespace PeliculasApiRestFul.DTOs
{
    public class PeliculaDetallesDTO : PeliculaDTO
    {
        public List<GeneroDTO> Generos { get; set; }
        public List<ActorPeliculaDetallesDTO> actorPeliculaDetallesDTOs { get; set; }
    }
}
