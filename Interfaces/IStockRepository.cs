using api.DTOs.Stocks;
using api.Helper;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
         Task<List<Stock>> GetAll(queryObject queryObject);

        Task<Stock> GetStockDetailsById(int id);

        Task<Stock> CreateStock(Stock stock);

        Task<Stock> UpdateStock(Stock stock, UpdateStockDto updateStockDto);
        Task<bool> DeleteStock(Stock stock);

        Task<bool> IsStockExist(int stockId);

    }
}
