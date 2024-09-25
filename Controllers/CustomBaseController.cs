using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApiRestFul.DTOs;
using PeliculasApiRestFul.Entidades;
using PeliculasApiRestFul.Helpers;

namespace PeliculasApiRestFul.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CustomBaseController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO) where TEntidad : class
        {
            var queryable = context.Set<TEntidad>().AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidad = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entidad);
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO,
            IQueryable<TEntidad> queryable)
            where TEntidad : class
        {
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entidades);
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad : class
        {
            var entidades = await context.Set<TEntidad>()
                .AsNoTracking().ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(entidades);
            return dtos;
        }

        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad : class, IId
        {
            var resultado = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (resultado == null) { return NotFound(); }
            return mapper.Map<TDTO>(resultado);
        }

        protected async Task<ActionResult> Post<TCreacionDTO, TEntidad, TDTO>(TCreacionDTO creacionDTO, string nombreRuta) where TEntidad : class, IId
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();

            var entidadDTO = mapper.Map<TDTO>(entidad);
            return new CreatedAtRouteResult(nombreRuta, new { id = entidad.Id }, entidadDTO);
        }
        protected async Task<ActionResult> Put<TCreacionDTO, TEntidad>(int id, TCreacionDTO creacionDTO)
            where TEntidad : class, IId
        {
            var genero = mapper.Map<TEntidad>(creacionDTO);
            genero.Id = id;
            context.Entry(genero).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
        protected async Task<ActionResult> Patch<TEntidad, TDTO>(int id, Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDTO> jsonPatchDocument) where TDTO : class
            where TEntidad : class, IId
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }
            var entidad = await context.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            var entidadDTO = mapper.Map<TDTO>(entidad);
            jsonPatchDocument.ApplyTo(entidadDTO, ModelState);
            var esValido = TryValidateModel(ModelState);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(entidadDTO, entidad);
            await context.SaveChangesAsync();
            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntidad>(int id) where TEntidad : class, IId, new()
        {
            var entidad = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);
            if (!entidad)
            {
                return NotFound();
            }
            context.Remove(new TEntidad() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
