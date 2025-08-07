using Masaafa.Application.Services;

namespace Masaafa.Infrastructure.Services;

public class CommandDispatcherService : ICommandDispatcherService
{
    public Task DispatchAsync(string message, long chatId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(message);

        if (message == "/start")
            return Task.CompletedTask;

        return Task.CompletedTask;
    }
}