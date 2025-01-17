using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalysis.Repository;
using Telegram.Bot;

namespace DataAnalysis.StateUser
{
    public class SearchResultsSender : ISearchResultsSender

    {
        private readonly IDatabaseRepository _databaseRepository;

        public SearchResultsSender(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }


        public async Task SendSearchReasultToUser(ITelegramBotClient botClient, long chatId, string searchQuery)
        {
            var products = await _databaseRepository.SearchProductsAsync(searchQuery);
            string productList = string.Join("\n", products.Select(p => $"{p.NameFa} - {p.NameEn}"));

            if (string.IsNullOrEmpty(productList))
            {
                productList = "محصولی یافت نشد";
            }


            const int maxMessageLength = 4096;

            while (productList.Length > maxMessageLength)
            {
                var part = productList.Substring(0, maxMessageLength); 
                await botClient.SendTextMessageAsync(chatId, part);
                productList = productList.Substring(maxMessageLength); 
            }


            await botClient.SendTextMessageAsync(chatId, productList);
        }
    }
}