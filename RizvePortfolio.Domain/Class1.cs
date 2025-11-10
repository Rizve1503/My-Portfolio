namespace RizvePortfolio.Domain.Entities;

/// <summary>
/// Base entity providing common audit properties.
/// </summary>
public abstract class BaseEntity
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
	public DateTime? ModifiedUtc { get; set; }
}
