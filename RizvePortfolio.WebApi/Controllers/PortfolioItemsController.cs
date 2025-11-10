using Microsoft.AspNetCore.Mvc;
using RizvePortfolio.Application.DTOs;
using RizvePortfolio.Application.Services;
using RizvePortfolio.WebApi.Models;

namespace RizvePortfolio.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioItemsController(IPortfolioService service) : ControllerBase
{
    private readonly IPortfolioService _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PortfolioItemResponse>>> GetAll(CancellationToken ct)
    {
        var items = await _service.ListAsync(ct);
        var resp = items.Select(x => new PortfolioItemResponse(x.Id, x.Title, x.Summary, x.Content, x.CategoryId)).ToList();
        return Ok(resp);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PortfolioItemResponse>> Get(Guid id, CancellationToken ct)
    {
        var x = await _service.GetAsync(id, ct);
        if (x is null) return NotFound();
        return Ok(new PortfolioItemResponse(x.Id, x.Title, x.Summary, x.Content, x.CategoryId));
    }

    [HttpPost]
    public async Task<ActionResult<PortfolioItemResponse>> Create([FromBody] CreatePortfolioItemRequest req, CancellationToken ct)
    {
        var dto = new PortfolioItemDto(Guid.Empty, req.Title, req.Summary, req.Content, req.CategoryId);
        var created = await _service.CreateAsync(dto, ct);
        var resp = new PortfolioItemResponse(created.Id, created.Title, created.Summary, created.Content, created.CategoryId);
        return CreatedAtAction(nameof(Get), new { id = resp.Id }, resp);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PortfolioItemResponse>> Update(Guid id, [FromBody] UpdatePortfolioItemRequest req, CancellationToken ct)
    {
        var dto = new PortfolioItemDto(id, req.Title, req.Summary, req.Content, req.CategoryId);
        var updated = await _service.UpdateAsync(id, dto, ct);
        if (updated is null) return NotFound();
        var resp = new PortfolioItemResponse(updated.Id, updated.Title, updated.Summary, updated.Content, updated.CategoryId);
        return Ok(resp);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
