using api.DTOs.Stocks;
using api.Models;

namespace api.DTOMappers
{
    public static class CretaStockDtoToModel
    {
        public static Stock CreateStockDtoToModelMaper(this CreateStockDto createStockDto)
        {
            return new Stock {

                StockId = createStockDto.StockId,
                Symbol = createStockDto.Symbol,
                CompanyName = createStockDto.CompanyName,
                Purchase = createStockDto.Purchase,
                LastDiv = createStockDto.LastDiv,
                Industry = createStockDto.Industry,
                MarketCap = createStockDto.MarketCap,


            };

        }
    }
}
