using System.Threading.Tasks;
using Bot.Telegram.API.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Bot.Telegram.API.Controllers
{
    public class BotWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromServices] IHandleUpdateService handleUpdateService,
            [FromBody] Update update)
        {
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}