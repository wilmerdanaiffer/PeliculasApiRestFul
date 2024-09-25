using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PeliculasApiRestFul.DTOs;
using PeliculasApiRestFul.Entidades;

namespace PeliculasApiRestFul
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<IdentityUser, UsuarioDTO>();
        }
    }
}
