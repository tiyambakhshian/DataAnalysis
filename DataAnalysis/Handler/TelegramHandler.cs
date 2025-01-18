using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalysis.StateUser;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;

namespace DataAnalysis.Handler
{
    public class TelegramHandler
    {
        private readonly IMessageHandle _handleMessage;
        private readonly ICallbackHandle _handleCallback;
        public TelegramHandler(IMessageHandle handleMessage, ICallbackHandle handleCallback)
        {
            _handleCallback = handleCallback;
            _handleMessage = handleMessage;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await _handleMessage.HandleMessage(botClient, update);
                    break;
                case UpdateType.CallbackQuery:
                    await _handleCallback.HandleCallback(botClient, update);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, System.Threading.CancellationToken cancellationToken)
            {
                Console.WriteLine($"Error occurred: {exception.Message}");
                return Task.CompletedTask;
            }

        }

}
