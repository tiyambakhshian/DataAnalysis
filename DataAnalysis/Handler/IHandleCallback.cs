using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace DataAnalysis.Handler
{
    public interface ICallbackHandle
    {
        Task HandleCallback(ITelegramBotClient botClient, Update update);
    }
}
