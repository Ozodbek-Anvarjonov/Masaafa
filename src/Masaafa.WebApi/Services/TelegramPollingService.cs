using Masaafa.Application.Common.Identity;
using Masaafa.Application.Services;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Masaafa.WebApi.Services;

public class TelegramPollingService(ITelegramBotClient botClient,
    IServiceScopeFactory scopedFactory,
    IMemoryCache memoryCache) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: stoppingToken
        );

        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (update.Type != UpdateType.Message || update.Message == null) return;

        var message = update.Message;
        var chatId = message.Chat.Id;
        var telegramUserId = message.From!.Id;

        using var scope = scopedFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

        var exist = await unitOfWork.Clients.Get()
            .Where(entity => entity.TelegramId == telegramUserId && message.Contact.PhoneNumber == entity.PhoneNumber && !entity.IsDeleted)
            .FirstOrDefaultAsync();

        if (exist is not null && !string.IsNullOrEmpty(exist.PhoneNumber))
        {
            var dispatchService = scopedFactory.CreateScope().ServiceProvider.GetRequiredService<ICommandDispatcherService>();
            await dispatchService.DispatchAsync(message.Text, chatId, ct);
            return;
        }

        await Register(message, chatId, telegramUserId, ct);
    }

    private async Task Register(Message message, long chatId, long telegramUserId, CancellationToken cancellationToken = default)
    {
        using var scope = scopedFactory.CreateScope();
    
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        if (message.Contact is not null && memoryCache.TryGetValue(telegramUserId, out RegisterForm form) && form.Step == 4)
        {
            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

            form.PhoneNumber = message.Contact.PhoneNumber;

            await accountService.SignUpAsync(new Client
            {
                TelegramId = form.Id,
                FirstName = form.FirstName,
                LastName = form.LastName,
                PhoneNumber = form.PhoneNumber,
                CardCode = form.CardCode,
            });

            await botClient.SendMessage(telegramUserId, "✅ Ro'yxatdan muvaffaqiyatli o‘tdingiz!");
            return;
        }

        var exists = await unitOfWork.Clients
            .Get()
            .AnyAsync(entity => entity.TelegramId == telegramUserId && !entity.IsDeleted);

        if (message.Text == "/start")
        {
            if (exists)
            {
                await botClient.SendMessage(telegramUserId, "Siz allaqachon ro'yxatdan o'tgansiz.");
                return;
            }

            var newForm = new RegisterForm { Id = telegramUserId, Step = 1 };

            memoryCache.Set(telegramUserId, newForm, TimeSpan.FromMinutes(10));
            await botClient.SendMessage(telegramUserId, "Iltimos, ismingizni yuboring:");
            return;
        }

        if (memoryCache.TryGetValue(telegramUserId, out var regStateForm))
        {
            var regForm = regStateForm as RegisterForm;
            switch (regForm.Step)
            {
                case 1:
                    regForm.FirstName = message.Text;
                    regForm.Step = 2;
                    await botClient.SendMessage(telegramUserId, "Iltimos familiyangizni kiriting:");
                    break;
                case 2:
                    regForm.LastName = message.Text;
                    regForm.Step = 3;
                    await botClient.SendMessage(telegramUserId, "Carta nomerni yuboring");
                    break;
                case 3:
                    regForm.CardCode = message.Text;
                    regForm.Step = 4;

                    var replyMarkup = new ReplyKeyboardMarkup(new[]
                    {
                        KeyboardButton.WithRequestContact("📱 Telefon raqamni yuborish")
                    })
                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true,
                    };
                    await botClient.SendMessage(telegramUserId, "Endi telefon raqamingizni yuboring:", replyMarkup: replyMarkup);
                    break;
                default:
                    await botClient.SendMessage(chatId, "Iltimos, /start buyrug'ini yuboring.");
                    break;
            }

            memoryCache.Set(telegramUserId, regForm);
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        Console.WriteLine($"Telegram error: {exception.InnerException}");
        return Task.CompletedTask;
    }
}

class RegisterForm
{
    public long Id { get; set; }

    public string ChatId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string CardCode { get; set; }

    public int Step { get; set; }
}