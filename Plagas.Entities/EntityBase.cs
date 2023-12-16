namespace Plagas.Entities;

public class EntityBase
{
    public int Id { get; set; }
    public bool Status { get; set; } // Esto sirve para la eliminacion logica (soft delete)

    protected EntityBase()
    {
        Status = true;
    }
}