using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalysis.Model;
using DataAnalysis.SearchUser;
using Telegram.Bot;


namespace DataAnalysis.StateUser
{
    public class SearchResultsSender : ISearchResultsSender

    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IMessagePartition _messagePartition;
        public SearchResultsSender(IDatabaseRepository databaseRepository, IMessagePartition messagePartition)
        {
            _databaseRepository = databaseRepository;
            _messagePartition = messagePartition;   
        }

        public IMessagePartition Get_messagePartition()
        {
            return _messagePartition;
        }

        public async Task SendSearchReasultToUser(ITelegramBotClient botClient, long chatId, string searchQuery)
        {
            var products = await _databaseRepository.SearchProductsAsync(searchQuery);

            
            if (products == null || !products.Any())
            {
                await botClient.SendTextMessageAsync(chatId, "محصولی یافت نشد.");
                return;
            }

            string productList = string.Join("\n", products.Select(p =>
                $"{(string.IsNullOrEmpty(p.ProductTitleFa) ? "عنوان فارسی ندارد" : p.ProductTitleFa)} - " +
                $"{(string.IsNullOrEmpty(p.ProductTitleEn) ? "عنوان انگلیسی ندارد" : p.ProductTitleEn)}"));


            if (string.IsNullOrEmpty(productList))
            {
                productList = "محصولی یافت نشد";
            }

            const int maxMessageLength = 4096;

            foreach (var part in _messagePartition.PartitionMessage(productList, maxMessageLength))
            {
                await botClient.SendTextMessageAsync(chatId, part);
            }
        }

    }
}