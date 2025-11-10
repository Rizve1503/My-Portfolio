namespace RizvePortfolio.Application.DTOs;

public record PortfolioItemDto(Guid Id, string Title, string? Summary, string? Content, Guid CategoryId);
