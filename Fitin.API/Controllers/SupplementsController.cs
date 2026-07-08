using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Supplements.Interfaces;
using Fitin.Application.Supplements.DTOs;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/supplements")]
public class SupplementsController : BaseApiController
{
    private readonly ISupplementService _supplementService;

    public SupplementsController(ISupplementService supplementService)
    {
        _supplementService = supplementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var supplements = await _supplementService.GetAllAsync();
        return Success(supplements, "Supplement products retrieved successfully");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var supplement = await _supplementService.GetByIdAsync(id);

        if (supplement == null)
            return Failure("Supplement product not found", null, 404);

        return Success(supplement, "Supplement product retrieved successfully");
    }
}
