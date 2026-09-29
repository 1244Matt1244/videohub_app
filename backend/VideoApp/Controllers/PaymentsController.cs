using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Commands.Payments;

namespace VideoApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaymentsController(IMediator mediator) => _mediator = mediator;

    private Guid UserId => Guid.Parse(User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException());

    [HttpPost("checkout")]
    public async Task<IActionResult> CreateCheckout([FromBody] CreateCheckoutCommand command)
    {
        var result = await _mediator.Send(command with { UserId = UserId });
        return result.Success ? Ok(new { url = result.Data }) : BadRequest(new { error = result.Error });
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].ToString();
        var result = await _mediator.Send(new ProcessStripeWebhookCommand(json, signature));
        return result.Success ? Ok() : BadRequest();
    }
}
