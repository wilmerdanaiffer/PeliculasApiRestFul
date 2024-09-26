using PeliculasApiRestFul.Validaciones;

namespace PeliculasApiRestFul.DTOs
{
    public class ActorCreacionDTO : ActorPatchDTO
    {
        [PesoArchivoValidacion(PesoMaxMbs: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile? Foto { get; set; }
    }
}
