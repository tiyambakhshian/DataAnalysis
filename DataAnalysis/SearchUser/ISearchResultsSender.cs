using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DataAnalysis.StateUser
{
    public interface ISearchResultsSender
    {
        Task SendSearchReasultToUser(ITelegramBotClient botClient, long chatId, string searchQuery);
    }
}
