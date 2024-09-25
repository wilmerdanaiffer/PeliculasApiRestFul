﻿namespace PeliculasApiRestFul.DTOs
{
    public class PaginacionDTO
    {
        public int pagina { get; set; } = 1;
        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int CantidadRegistrosPorPagina
        {
            get => cantidadRegistrosPorPagina;
            set
            {
                cantidadRegistrosPorPagina = (cantidadMaximaRegistrosPorPagina < value) ? cantidadMaximaRegistrosPorPagina : value;
            }
        }
    }
}
