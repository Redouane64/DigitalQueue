using System.Text.Json;

using DigitalQueue.Web.Areas.Accounts.Commands;

using StackExchange.Redis;

namespace DigitalQueue.Web.Services.Notifications;

public class NotificationService : RedisPubSubService
{
    private readonly string ChannelName = "Notifications";
    private readonly MailService _mailService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IConnectionMultiplexer connectionMultiplexer,
        MailService mailService,
        ILogger<NotificationService> logger)
        : base(connectionMultiplexer)
    {
        _mailService = mailService;
        _logger = logger;
        Subscribe(ChannelName);
    }

    protected override async Task OnMessage(ChannelMessage channelMessage)
    {
        var message = channelMessage.Message.ToString();
        var payload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);

        var payloadType = payload.GetValueOrDefault("Type");
        
        switch (payloadType.GetString())
        {
            case nameof(SendEmailConfirmation):

                var sendEmailConfirmationRaw = payload.GetValueOrDefault("Payload");
                var sendEmailConfirmation = sendEmailConfirmationRaw.Deserialize<SendEmailConfirmation>();

                switch (sendEmailConfirmation.Method)
                {
                    case CreateEmailConfirmationTokenCommand.ConfirmationMethod.Url:
                        await this._mailService.SendEmailConfirmationUrl(sendEmailConfirmation.Email, sendEmailConfirmation.Token);
                        break;
                    case CreateEmailConfirmationTokenCommand.ConfirmationMethod.Code:
                        await this._mailService.SendEmailConfirmationCode(sendEmailConfirmation.Email, sendEmailConfirmation.Token);
                        break;
                }
                
                break;
            
            case nameof(SendPasswordResetCode):
                var sendPasswordResetRaw = payload.GetValueOrDefault("Payload");
                var passwordReset = sendPasswordResetRaw.Deserialize<SendPasswordResetCode>();
                
                await this._mailService.SendPasswordResetCode(passwordReset.Email, passwordReset.Token);
                
                break;
        }
    }

    public Task Publish<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        return base.Publish(ChannelName, json);
    }

}
