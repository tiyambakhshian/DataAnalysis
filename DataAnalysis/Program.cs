using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using DataAnalysis.Handler;
using DataAnalysis.StateUser;
using Microsoft.Extensions.DependencyInjection;
using DataAnalysis;
using DataAnalysis.Repository;

class Program
{

    private static TelegramBotClient botClient;


    static async Task Main(string[] args)
    {
        const string token = "8116189035:AAHON2kbFotSnnxh-67BccqCGzpvwnjismE";

        var botClient = new TelegramBotClient(token);

        // ایجاد Container برای Dependency Injection
        var serviceProvider = new ServiceCollection()
        .AddSingleton<IUserSearchState, UserSearchState>()
        .AddSingleton<ISearchResultsSender, SearchResultsSender>()
        .AddSingleton<IDatabaseRepository, DatabaseRepository>()
        .AddSingleton<ITelegramBotClient>(botClient)
        .AddSingleton<TelegramHandler>()
        .BuildServiceProvider();


        // گرفتن نمونه از TelegramHandler از DI Container
        var handler = serviceProvider.GetService<TelegramHandler>();

        if (handler == null)
        {
            Console.WriteLine("Error: TelegramHandler not found.");
            return;
        }

        // شروع دریافت آپدیت‌ها از تلگرام
        var cts = new CancellationTokenSource();
        botClient.StartReceiving(
            updateHandler: handler.HandleUpdateAsync,
            errorHandler: handler.HandleErrorAsync,
            cancellationToken: cts.Token
        );

        Console.WriteLine("Bot is running...");
        Console.ReadLine();
        cts.Cancel();
    }

    //private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, System.Threading.CancellationToken cancellationToken)
    //{
    //    if (update.Type == UpdateType.Message)
    //    {
    //        var message = update.Message;
    //        if (message != null && message.Text != null)
    //        {
    //            string userMessage = message.Text.ToLower();

    //            //زدن کاربر"/start" 
    //            if (userMessage == "/start")
    //            {
    //                // دو تا دکمه واسه کاستوم کردن عملکرد بات
    //                var keyboard = new InlineKeyboardMarkup(
    //                    new[]
    //                    {
    //                        new[]
    //                        {
    //                            InlineKeyboardButton.WithCallbackData("جستجوی محصول", "search"),
    //                            InlineKeyboardButton.WithCallbackData("درباره ربات من", "about")
    //                        }
    //                    });

    //                await botClient.SendTextMessageAsync(
    //                    chatId: message.Chat.Id,
    //                    text: "خیلی خوش اومدی ، یه گزینه رو انتخاب کن!?",
    //                    replyMarkup: keyboard
    //                );
    //            }

    //            else if (userSearchStates.ContainsKey(message.Chat.Id) && userSearchStates[message.Chat.Id] == "waiting_for_search")
    //            {

    //                string searchQuery = message.Text.Trim();
    //                if (!string.IsNullOrEmpty(searchQuery))
    //                {

    //                    await SendSearchResultsToUser(message.Chat.Id, searchQuery);
    //                    userSearchStates[message.Chat.Id] = string.Empty;
    //                }
    //                else
    //                {
    //                    await botClient.SendTextMessageAsync(
    //                        chatId: message.Chat.Id,
    //                        text: "اسم محصولی که دنبالشی رو وارد کن."
    //                    );
    //                }
    //            }
    //        }
    //    }

    //    else if (update.Type == UpdateType.CallbackQuery)
    //    {
    //        var callbackQuery = update.CallbackQuery;

    //        // عملکرد دکمه ها
    //        if (callbackQuery.Data == "search")
    //        {

    //            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "جهت جستجو نام محصول را وارد کنید.");
    //            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "اسم محصولی که دنبالشی رو وارد کن.");


    //            userSearchStates[callbackQuery.Message.Chat.Id] = "waiting_for_search";
    //        }
    //        else if (callbackQuery.Data == "about")
    //        {
    //            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "این ربات برای جستجوی محصولات طراحی شده است.");          
    //            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "درباره بات : این بات رو تیام ساخته و هدفش این بوده که بتونین اسم محصولاتی که میخواین رو توی دیجیکالا سرچ کنین.");
    //        }
    //    }
    //}


    //private static async Task SendSearchResultsToUser(long chatId, string searchQuery)
    //{
    //    string connectionString = "Server=localhost; Database=DataAnalysis; Integrated Security=True;"; 
    //    string query = "SELECT product_title_fa, product_title_en FROM  dbo.Product WHERE product_title_fa LIKE @searchQuery";

    //    using (SqlConnection connection = new SqlConnection(connectionString))
    //    {
    //        await connection.OpenAsync();

    //        using (SqlCommand command = new SqlCommand(query, connection))
    //        {
    //            command.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

    //            using (SqlDataReader reader = await command.ExecuteReaderAsync())
    //            {

    //                string productList = "نتایج سرچی که زدی:\n";
    //                bool foundResults = false;

    //               while (await reader.ReadAsync())
    //                {
                       
    //                   string productNameFa = reader["product_title_fa"] as string; 
    //                   string productNameEn = reader["product_title_en"] as string;
    //                while (await reader.ReadAsync())
    //                {

    //                    string productNameFa = reader["product_title_fa"] as string; 
    //                    string productNameEn = reader["product_title_en"] as string;

    //                    if (string.IsNullOrEmpty(productNameFa))
    //                    {
    //                        productNameFa = "بدون نام";
    //                    }

    //                    if (productNameEn == null) 
    //                  {
    //                       productNameEn = 0;
    //                  }

    //                    productList += $"{productNameFa} - {productNameEn} \n";
    //                    foundResults = true;
    //                }

    //                if (!foundResults)
    //                {
    //                    productList = "محصولی با این نامی که وارد کردی پیدا نکردم.";
    //                }

    //                int maxMessageLength = 4096;
    //                while (productList.Length > maxMessageLength)
    //                {

    //                    string part = productList.Substring(0, maxMessageLength);

    //                    await botClient.SendTextMessageAsync(chatId, part);

    //                    productList = productList.Substring(maxMessageLength);
    //                }

    //                await botClient.SendTextMessageAsync(chatId, productList);
    //            }
    //        }
    //    }
    //}


    //private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, System.Threading.CancellationToken cancellationToken)
    //{
    //    Console.WriteLine($"Error occurred: {exception.Message}");
    //    return Task.CompletedTask;
    //}
}
