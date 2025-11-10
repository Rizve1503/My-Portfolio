namespace RizvePortfolio.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation
    public ICollection<PortfolioItem> Items { get; set; } = new List<PortfolioItem>();
}
