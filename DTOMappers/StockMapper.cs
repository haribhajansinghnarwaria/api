using api.DTOs.Stocks;
using api.Models;
using System.Runtime.CompilerServices;

namespace api.DTOMappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto( this Stock stockModel)
        {
            return new StockDto {

                StockId = stockModel.StockId, 
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase   =    stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap  = stockModel.MarketCap,


               };

        }
    }
}
