using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApiRestFul.DTOs;
using PeliculasApiRestFul.Entidades;
using PeliculasApiRestFul.Helpers;
using PeliculasApiRestFul.Servicios;

namespace PeliculasApiRestFul.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivosAzure;
        private readonly ILogger<PeliculasController> logger;
        private readonly string contenedor = "public-images";

        public PeliculasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivosAzure, ILogger<PeliculasController> logger)
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivosAzure = almacenadorArchivosAzure;
            this.logger = logger;
        }

        [HttpGet(Name = "ObtenerPeliculas")]
        public async Task<ActionResult<PeliculasIndexDTO>> Get()
        {
            var proximosExtrenos = await context.Peliculas
                .Where(x => x.FechaEstreno > DateTime.Today)
                .OrderBy(x => x.FechaEstreno)
                .Take(5)
                .ToListAsync();

            var enCines = await context.Peliculas
                .Where(x => x.EnCines)
                .Take(8)
                .ToListAsync();
            var resultado = new PeliculasIndexDTO();
            resultado.ProximosExtrenos = mapper.Map<List<PeliculaDTO>>(proximosExtrenos);
            resultado.PeliculasEnCine = mapper.Map<List<PeliculaDTO>>(enCines);

            return resultado;
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] FiltroPeliculasDTO filtroPeliculasDTO)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(filtroPeliculasDTO.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(filtroPeliculasDTO.Titulo));
            }
            if (filtroPeliculasDTO.EnCines)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCines);
            }
            if (filtroPeliculasDTO.ProximosEstrenos)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaEstreno > DateTime.Today);
            }
            if (filtroPeliculasDTO.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.PeliculasGeneros
                .Select(y => y.GeneroId).Contains(filtroPeliculasDTO.GeneroId));
            }
            if (!string.IsNullOrEmpty(filtroPeliculasDTO.CampoOrdenarPor))
            {
                var tipoOrden = filtroPeliculasDTO.OrdenAscendente ? "ascending" : "descending";
                try
                {
                    //peliculasQueryable = peliculasQueryable.OrderBy($"{filtroPeliculasDTO.CampoOrdenarPor} {tipoOrden}");
                }
                catch (Exception ex)
                {

                    logger.LogError(ex.Message, ex);
                }
            }

            await HttpContext.InsertarParametrosPaginacion(peliculasQueryable, filtroPeliculasDTO.CantidadRegistrosPorPagina);
            var peliculas = await peliculasQueryable.Paginar(filtroPeliculasDTO.paginacion)
                .ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);

        }

        [HttpGet("{id:int}", Name = "ObtenerPelicula")]
        public async Task<ActionResult<PeliculaDetallesDTO>> Get(int id)
        {
            var entidad = await context.Peliculas
                .Include(x => x.PeliculasActores).ThenInclude(x => x.actor)
                .Include(x => x.PeliculasGeneros).ThenInclude(y => y.genero)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            entidad.PeliculasActores = entidad.PeliculasActores.OrderBy(x => x.Orden).ToList();
            return mapper.Map<PeliculaDetallesDTO>(entidad);
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i + 1;
                }
            }
        }

        [HttpPost(Name = "CrearPelicula")]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var entidadDB = mapper.Map<Pelicula>(peliculaCreacionDTO);
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    entidadDB.Poster = await almacenadorArchivosAzure.GuardarArchivo(contenido, extension, contenedor, peliculaCreacionDTO.Poster.ContentType);
                }
            }
            AsignarOrdenActores(entidadDB);
            context.Add(entidadDB);
            await context.SaveChangesAsync();
            var peliculaDTO = mapper.Map<PeliculaDTO>(entidadDB);
            return new CreatedAtRouteResult("ObtenerPelicula", new { id = entidadDB.Id }, peliculaDTO);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDB = context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculasGeneros)
                .FirstOrDefault(x => x.Id == id);
            if (peliculaDB == null)
            {
                return NotFound();
            }
            mapper.Map(peliculaCreacionDTO, peliculaDB);
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivosAzure.EditarArchivo(contenido, extension, contenedor, peliculaDB.Poster, peliculaCreacionDTO.Poster.ContentType);
                }
            }
            AsignarOrdenActores(peliculaDB);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<PeliculaPatchDTO> jsonPatchDocument)
        {
            return await Patch<Pelicula, PeliculaPatchDTO>(id, jsonPatchDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Pelicula>(id);
        }


    }
}
