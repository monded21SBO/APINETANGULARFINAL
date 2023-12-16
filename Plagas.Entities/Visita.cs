
using System;

namespace Plagas.Entities
{
    public class Visita : EntityBase


    {
        public Tecnicos Tecnico { get; set; } = default!;
        public int TecnicoId { get; set; }
        public Trampas Trampa { get; set; } = default!;
        public int TrampaId { get; set; }
        public DateTime FechaVisita { get; set; }
        public string OperationNumber { get; set; } = default!;
        public string? Comentarios { get; set; }
        public string? ImageUrl { get; set; }


    }
}
