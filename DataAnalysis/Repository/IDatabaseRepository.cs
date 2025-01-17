using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.Repository
{
    public interface IDatabaseRepository
    {
        Task<List<Product>> SearchProductsAsync(string searchQuery);
    }
}

