using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalysis.Model;
using Microsoft.EntityFrameworkCore;

public class DatabaseRepository : IDatabaseRepository
{
    private readonly DataAnalysisContext _context;

    public DatabaseRepository(DataAnalysisContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> SearchProductsAsync(string searchQuery)
    {


        if (string.IsNullOrEmpty(searchQuery))
        {
            Console.WriteLine("The search query is empty.");
            return new List<Product>();  
        }

        var products = await _context.Products
                                       .Where(p => p.ProductTitleFa.ToLower().Contains(searchQuery.ToLower()))
                                       .Take(50)
                                       .ToListAsync();


        return products;
    }


}

