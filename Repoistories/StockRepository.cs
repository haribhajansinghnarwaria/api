using api.Data;
using api.DTOs.Stocks;
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

        public async Task<Stock> GetStockDetailsById(int id) {
            var stock = await _context.Stock.FindAsync(id);
            return stock;

        }
        public async Task<Stock> CreateStock(Stock _stockModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Stock ON;");
                await _context.Stock.AddAsync(_stockModel);
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Stock OFF;");
                await transaction.CommitAsync();
            }

            return _stockModel;
        }

        public async Task<Stock> UpdateStock(Stock stockModel, UpdateStockDto updateStockDto)
        {
            stockModel.Symbol = updateStockDto.Symbol;
            stockModel.MarketCap = updateStockDto.MarketCap;
            stockModel.Purchase = updateStockDto.Purchase;
            stockModel.CompanyName = updateStockDto.CompanyName;
            stockModel.LastDiv = updateStockDto.LastDiv;
            stockModel.Industry = updateStockDto.Industry;
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<bool> DeleteStock(Stock stock)
        {
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
