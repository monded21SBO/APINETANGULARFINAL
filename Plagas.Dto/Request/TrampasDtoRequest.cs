

namespace Plagas.Dto.Request
{
    public class TrampasDtoRequest
    {
        public string Nombre { get; set; } = default!;
        public string Descripcion { get; set; } = default!;
        public string Ubicacion { get; set; } = default!;
        public int TiposId { get; set; }
        public DateTime FechaInstalacion { get; set; }
        public string? ImageUrl { get; set; }
        public string Responsable { get; set; } = default!;
        public string? Base64Image { get; set; }
        public string? FileName { get; set; }

    }
}
