using api.Data;
using api.DTOs.Stocks;
using api.Helper;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace api.Repoistories
{
    public class StockRepository : IStockRepository
    {
        public readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context) //Constructor Dependency Injection
        {
            _context = context; 
        }
        public async Task<List<Stock>> GetAll(queryObject queryObject)
        {
            //var stock = await _context.Stock.ToListAsync();
            //var stock = await _context.Stock.Include(c => c.Comments).ToListAsync();
            //return stock;
            //Filtering 
            var stocks = _context.Stock.Include(c => c.Comments).AsQueryable();// When we are using AsQueryAble here it is not firing the query to db it jus preparing the qyery
            if (!String.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stocks = stocks.Where(c => c.Symbol.Contains(queryObject.Symbol));
            }
            if (!String.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = stocks.Where(c => c.CompanyName.Contains(queryObject.CompanyName));
            }
            //Sorting logic
            if(!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                if (queryObject.SortBy.Equals("Symbol"))
                {
                   stocks =  queryObject.IsDescending ? stocks.OrderByDescending(c => c.Symbol) : stocks.OrderBy(c => c.Symbol);
                }
                if (queryObject.SortBy.Equals("CompanyName"))
                {
                    stocks = queryObject.IsDescending ? stocks.OrderByDescending(c => c.CompanyName) : stocks.OrderBy(c => c.CompanyName);
                }
            }
            //Pagination
            var skipNumber = (queryObject.PageNumber - 1)*queryObject.PageSize;
            if (skipNumber > 0)
            {
                return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync(); // now at this point fire the SQL query to the db


            }
            return await stocks.ToListAsync(); // now at this point fire the SQL query to the db
        }

        public async Task<Stock> GetStockDetailsById(int id) {
            //var stock = await _context.Stock.FindAsync(id);
            var stock = await _context.Stock
                .Where(s => s.StockId == id)
                .Include(c => c.Comments).FirstOrDefaultAsync();
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

        public async Task<bool> IsStockExist(int stockId)
        {
           return await  _context.Stock.AnyAsync(c => c.StockId == stockId);
        }
    }
}
