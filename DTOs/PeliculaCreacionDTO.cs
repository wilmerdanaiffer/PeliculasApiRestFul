using Microsoft.AspNetCore.Mvc;
using PeliculasApiRestFul.Helpers;
using PeliculasApiRestFul.Validaciones;

namespace PeliculasApiRestFul.DTOs
{
    public class PeliculaCreacionDTO : PeliculaPatchDTO
    {
        [PesoArchivoValidacion(PesoMaxMbs: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile? Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int>? GenerosIDs { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculasCreacionDTO>>))]
        public List<ActorPeliculasCreacionDTO>? Actores { get; set; }
    }
}
