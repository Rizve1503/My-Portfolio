namespace RizvePortfolio.WebApi.Models;

public record CreatePortfolioItemRequest(string Title, string? Summary, string? Content, Guid CategoryId);
public record UpdatePortfolioItemRequest(string Title, string? Summary, string? Content, Guid CategoryId);
public record PortfolioItemResponse(Guid Id, string Title, string? Summary, string? Content, Guid CategoryId);
