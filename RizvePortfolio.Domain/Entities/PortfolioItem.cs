namespace RizvePortfolio.Domain.Entities;

public class PortfolioItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public Guid CategoryId { get; set; }

    // Navigation
    public Category? Category { get; set; }
}
