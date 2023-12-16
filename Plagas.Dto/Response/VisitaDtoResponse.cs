

namespace Plagas.Dto.Response
{
    public class VisitaDtoResponse
    {
        public int VisitaId { get; set; }

        public string FechaInstalacion { get; set; } = default!;

        public string Trampas { get; set; } = default!;

        public string? ImageUrl { get; set; }

        public string Nombre { get; set; } = default!;

        public string OperationNumber { get; set; } = default!;
        public string FullName { get; set; } = default!;

        public string FechaVisita { get; set; } = default!;

        public string? Comentarios { get; set; }

  

    }
}
