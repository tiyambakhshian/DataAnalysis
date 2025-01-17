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
        private readonly ISearchResultsSender _searchResultsSender;
        private readonly IUserSearchState _userSearchState;
        public TelegramHandler(ISearchResultsSender searchResultsSender, IUserSearchState userSearchState)
        {
            _searchResultsSender = searchResultsSender;
            _userSearchState = userSearchState;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await HandleMessage(botClient, update);
                    break;
                case UpdateType.CallbackQuery:
                    await HandleCallback(botClient, update);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private async Task HandleMessage(ITelegramBotClient botClient, Update update)
        {
            var message = update.Message;
            if (message is null || message.Text is null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Command is invalid"
                );
                return;
            }

            string userMessage = message.Text.ToLower();

            if (userMessage == "/start")
            {
                var keyboard = new InlineKeyboardMarkup(
                    new[]
                    {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("جستجوی محصول", "search"),
                        InlineKeyboardButton.WithCallbackData("درباره ربات من", "about")
                    }
                    });

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "خیلی خوش اومدی ، یه گزینه رو انتخاب کن!?",
                    replyMarkup: keyboard
                );
            }
            else if (_userSearchState.IsWaitingForSearch(message.Chat.Id))
            {
                string searchQuery = message.Text.Trim();
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    await _searchResultsSender.SendSearchReasultToUser(botClient, message.Chat.Id, searchQuery);
                    _userSearchState.ClearSearchState(message.Chat.Id);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "اسم محصولی که دنبالشی رو وارد کن."
                    );
                }
            }
        }
        private async Task HandleCallback(ITelegramBotClient botClient, Update update)
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
    
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error occurred: {exception.Message}");
            return Task.CompletedTask;
        }

    }
}
