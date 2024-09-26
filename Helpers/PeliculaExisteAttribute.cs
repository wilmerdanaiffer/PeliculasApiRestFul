using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PeliculasApiRestFul.Helpers
{
    public class PeliculaExisteAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly ApplicationDbContext applicationDbContext;

        public PeliculaExisteAttribute(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var peliculaIdObject = context.HttpContext.Request.RouteValues["peliculaId"];
            if (peliculaIdObject == null)
            {
                return;
            }
            var peliculaId = int.Parse(peliculaIdObject.ToString());
            var existePelicula = await applicationDbContext.Peliculas.AnyAsync(x => x.Id == peliculaId);
            if (!existePelicula)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
            }

        }
    }
}
