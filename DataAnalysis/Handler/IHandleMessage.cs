using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DataAnalysis.Handler
{
    public interface IMessageHandle
    {
        Task HandleMessage(ITelegramBotClient botClient, Update update);
    }
}
