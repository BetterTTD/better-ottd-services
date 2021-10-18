using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Bot.Telegram.API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendMessage(
            [FromServices] ILogger<TestController> logger,
            [FromServices] ITelegramBotClient botClient,
            [FromBody] SendMessageRequest request)
        {
            try
            {
                if (request.ChatId is null || request.Message is null)
                    return BadRequest();
                
                await botClient.SendTextMessageAsync(request.ChatId, request.Message);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "SendMessage error");
                throw;
            }
        }
    }

    public sealed class SendMessageRequest
    {
        public long? ChatId { get; set; } = null;
        public string? Message { get; set; } = null;
    }
}
