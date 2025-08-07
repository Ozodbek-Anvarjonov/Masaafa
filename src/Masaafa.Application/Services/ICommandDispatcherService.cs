namespace Masaafa.Application.Services;

public interface ICommandDispatcherService
{
    Task DispatchAsync(string message, long chatId, CancellationToken cancellationToken = default);
}