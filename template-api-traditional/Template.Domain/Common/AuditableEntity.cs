namespace Template.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime? FechaCreacion { get; set; } = new DateTime(2022, 04, 18);
    public DateTime? FechaModificacion { get; set; } = null;
    public bool? Eliminado { get; set; } = false;
    public DateTime? FechaEliminacion { get; set; } = null;
}