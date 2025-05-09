using api.Data;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;


public class StockRepository : IStockRepository
{

    private readonly ApplicationDBContext Context;
    public StockRepository(ApplicationDBContext context)
    {
        Context = context;
    }
    
        public Task<List<Stock>> GetAllAsync()
    {
       return Context.Stocks.ToListAsync();
    }
}