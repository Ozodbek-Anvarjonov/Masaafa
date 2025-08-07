using Masaafa.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Masaafa.WebApi.Controllers;

public class WebHookController(ITelegramBotClient botClient, ICommandDispatcherService dispatcherService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        if (update.Message is not null)
        {
            var message = update.Message.Text!;
            var chatId = update.Message.Chat.Id;

            await dispatcherService.DispatchAsync(message, chatId, CancellationToken);
        }

        return Ok();
    }
}