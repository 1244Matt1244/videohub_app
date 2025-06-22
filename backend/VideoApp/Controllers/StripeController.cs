using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using System.IO;
using System.Threading.Tasks;
using VideoApp.Services;
using VideoApp.Interfaces;

namespace VideoApp.Controllers
{
    [Route("api/stripe")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IVideoService _videoService;

        public StripeController(IConfiguration config, IVideoService videoService)
        {
            _config = config;
            _videoService = videoService;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = 999, // u centima (npr. $9.99 = 999)
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "videoId", request.VideoId }
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var secret = _config["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], secret);

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var intent = stripeEvent.Data.Object as PaymentIntent;
                    if (intent?.Metadata != null &&
                        intent.Metadata.TryGetValue("videoId", out var videoId))
                    {
                        await _videoService.UnlockPremiumVideo(videoId);
                    }
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest($"Stripe greška: {ex.Message}");
            }
        }
    }

    // StripeController.cs
    public class PaymentRequest
    {
        public string VideoId { get; set; } = string.Empty;
    }
}
