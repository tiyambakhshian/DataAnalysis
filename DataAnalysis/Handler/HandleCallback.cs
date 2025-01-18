using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using DataAnalysis.StateUser;

namespace DataAnalysis.Handler
{
    public class CallbackHandle : ICallbackHandle
    {
        private readonly IUserSearchState _userSearchState;
        
        public CallbackHandle(IUserSearchState userSearchState)
        {
            _userSearchState = userSearchState;
        }
        public async Task HandleCallback(ITelegramBotClient botClient, Update update)
        {
            var callbackQuery = update.CallbackQuery;
            if (callbackQuery.Data == "search")
            {
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "جهت جستجو نام محصول را وارد کنید.");
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "اسم محصولی که دنبالشی رو وارد کن.");
                _userSearchState.SetSearchState(callbackQuery.Message.Chat.Id, "waiting_for_search");
            }
            else if (callbackQuery.Data == "about")
            {
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "این ربات برای جستجوی محصولات طراحی شده است.");
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "درباره بات : این بات رو تیام ساخته و هدفش این بوده که بتونین اسم محصولاتی که میخواین رو توی دیجیکالا سرچ کنین.");
            }
        }
    }
}
