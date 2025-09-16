using Calculator.Api.Models;
using Calculator.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CalculatorController(ICalculateUseCase useCase) : ControllerBase
{
    private readonly ICalculateUseCase _useCase = useCase;

    [HttpPost]
    public IActionResult Calculate(
        [FromBody] OperationRequest body,
        [FromHeader(Name = "X-Operation")] string operation
    )
    {
        if (
            !Request.Headers.TryGetValue("X-Operation", out var op) || string.IsNullOrWhiteSpace(op)
        )
            return BadRequest("Missing required header: X-Operation");

        try
        {
            var result = _useCase.Execute(body.Number1, body.Number2, op!);
            return Ok(new { result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
