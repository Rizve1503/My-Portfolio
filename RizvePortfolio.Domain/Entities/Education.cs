namespace RizvePortfolio.Domain.Entities;

public class Education
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
}
