using Masaafa.Application.Common.Notifications;
using Masaafa.Application.Extensions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Masaafa.Infrastructure.Common.Notifications;

public class MessageSenderService(ITelegramBotClient botClient) : IMessageSenderService
{
    public async Task<bool> SendAsync(string message, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendMessage(chatId, message);

        return true;
    }

    public async Task<bool> SendAsync(string message, List<long> chatIds, CancellationToken cancellationToken = default)
    {
        if (chatIds is null || chatIds.Count == 0)
            return true;

        var batches = chatIds.Batch(30);

        foreach (var batch in batches)
        {
            batch.Select(chatId =>
                botClient.SendMessage(chatId, message));

            await Task.WhenAll();
            await Task.Delay(1000);
        }

        return true;
    }

    public async Task<bool> SendWithRetryAsync(string message, long chatId, int maxAttempts, CancellationToken cancellationToken = default)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await botClient.SendMessage(chatId, message);
                return true;
            }
            catch (ApiRequestException ex) when (ex.ErrorCode == 429)
            {
                var waitSeconds = ex.Parameters?.RetryAfter ?? 3;
                await Task.Delay(waitSeconds * 1000);
            }
            catch (Exception ex)
            {
                await Task.Delay(1000); // Wait a bit before retry
            }
        }
        return false;
    }
}