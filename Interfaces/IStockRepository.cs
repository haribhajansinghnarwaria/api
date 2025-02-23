using api.DTOs.Stocks;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
         Task<List<Stock>> GetAll();

        Task<Stock> GetStockDetailsById(int id);

        Task<Stock> CreateStock(Stock stock);

        Task<Stock> UpdateStock(Stock stock, UpdateStockDto updateStockDto);
        Task<bool> DeleteStock(Stock stock);

    }
}
