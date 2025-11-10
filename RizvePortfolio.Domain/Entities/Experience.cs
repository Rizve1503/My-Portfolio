namespace RizvePortfolio.Domain.Entities;

public class Experience
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Company { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
    public string? Description { get; set; }
}
