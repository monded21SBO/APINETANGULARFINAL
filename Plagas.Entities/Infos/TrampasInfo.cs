namespace Plagas.Entities.Infos;

public class TrampasInfo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = default!;
    public string Descripcion { get; set; } = default!;
    public string Ubicacion { get; set; } = default!;
    public string Tipos { get; set; } = default!;
    public int TiposId { get; set; }
    public string FechaInstalacion { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string Responsable { get; set; } = default!;
    public string Status { get; set; } = default!;
}