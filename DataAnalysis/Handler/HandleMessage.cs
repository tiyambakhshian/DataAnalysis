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
    public class MessageHandle : IMessageHandle
    {
        private readonly ISearchResultsSender _searchResultsSender;
        private readonly IUserSearchState _userSearchState;

        public MessageHandle(ISearchResultsSender searchResultsSender, IUserSearchState userSearchState)
        { 
            _searchResultsSender = searchResultsSender;
            _userSearchState = userSearchState; 
        }
        public async Task HandleMessage(ITelegramBotClient botClient, Update update)
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
                Console.WriteLine($"Searching for: {searchQuery}");
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
    }
}
