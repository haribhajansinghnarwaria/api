using api.Data;
using api.DTOMappers;
using api.DTOs.Stocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;//read only as we don't want to modify this ofject
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var stock = _context.Stock.ToList();// will return a list of Stock Entity class

            var stock = await _context.Stock.ToListAsync();
             var st =   stock.Select(x => x.ToStockDto()); // now it will return an array of StockDto class
            return Ok(st);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockDetailsById([FromRoute]int id) 
        { 
            var stock = await _context.Stock.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            //return Ok(stock); this is will return stock Entity  Comlete Model
            return Ok(stock.ToStockDto());// this will return now DTO with trimmed properties of Entity Class
        
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDto createStockDto)
        {
           var  _stockModel = CretaStockDtoToModel.CreateStockDtoToModelMaper(createStockDto);
            using (var transaction = _context.Database.BeginTransaction())
            {
               await  _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Stock ON;");
               await _context.Stock.AddAsync(_stockModel);
               await _context.SaveChangesAsync();
               await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Stock OFF;");
               await transaction.CommitAsync();
            }

            return Ok(createStockDto);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpadeStock([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.StockId == id);
            if (stockModel == null)
            {
                return NotFound($"Did not found object with object Id {id}");
            }
            stockModel.Symbol = updateStockDto.Symbol;
            stockModel.MarketCap = updateStockDto.MarketCap;
            stockModel.Purchase = updateStockDto.Purchase;
            stockModel.CompanyName = updateStockDto.CompanyName;
            stockModel.LastDiv  = updateStockDto.LastDiv;
            stockModel.Industry = updateStockDto.Industry;
           await _context.SaveChangesAsync();
            return Ok(updateStockDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(x => x.StockId == id);
            if (stock == null) return NotFound();
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
