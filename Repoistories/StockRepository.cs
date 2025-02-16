using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repoistories
{
    public class StockRepository : IStockRepository
    {
        public readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context) //Constructor Dependency Injection
        {
            _context = context; 
        }
        public async Task<List<Stock>> GetAll()
        {
            var stock = await _context.Stock.ToListAsync();
            return stock;
        }
    }
}
