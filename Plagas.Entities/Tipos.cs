using System.ComponentModel.DataAnnotations;

namespace Plagas.Entities
{
    public class Tipos : EntityBase
    {
        [StringLength(100)] // DataAnnotations tiene menos prioridad que el Fluent API
        public string Name { get; set; } = default!;

    }
}