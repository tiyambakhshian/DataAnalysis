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
using DataAnalysis.Model;
using DataAnalysis.SearchUser;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


class Program
{

    private static TelegramBotClient botClient;


    static async Task Main(string[] args)
    {
        const string token = "8116189035:AAHON2kbFotSnnxh-67BccqCGzpvwnjismE";

        var botClient = new TelegramBotClient(token);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // ایجاد Container برای Dependency Injection
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IUserSearchState, UserSearchState>()
            .AddSingleton<ISearchResultsSender, SearchResultsSender>()
            .AddSingleton<IDatabaseRepository, DatabaseRepository>()
            .AddSingleton<IMessageHandle, MessageHandle>()
            .AddSingleton<ICallbackHandle, CallbackHandle>()
            .AddSingleton<IMessagePartition, MessagePartition>()
            .AddSingleton<ITelegramBotClient>(botClient)
            .AddSingleton<TelegramHandler>()
            .AddDbContext<DataAnalysisContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
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
}
