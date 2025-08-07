namespace Masaafa.Application.Common.Notifications;

public interface IMessageSenderService
{
    Task<bool> SendAsync(string message, long chatId, CancellationToken cancellationToken = default);

    Task<bool> SendWithRetryAsync(string message, long chatId, int maxAttempts, CancellationToken cancellationToken = default);

    Task<bool> SendAsync(string message, List<long> chatIds, CancellationToken cancellationToken = default);
}