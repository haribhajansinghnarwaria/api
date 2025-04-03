using api.DTOMappers;
using api.DTOs.Stocks;
using api.Helper;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        //private readonly ApplicationDBContext _context;//read only as we don't want to modify this ofject
        private readonly IStockRepository _stockRepository;
        private readonly IRedisCacheService _redisCacheService;
        public StockController(IStockRepository stockRepository, IRedisCacheService redisCacheService)
        {
            //_context = context;
            _stockRepository = stockRepository;
            _redisCacheService = redisCacheService;
        }

        [HttpGet]
        [Authorize] // adding this attribute to enforce authorization by forcefully using JWT to access this method.
        public async Task<IActionResult> GetAll([FromQuery] queryObject queryObject)
        {
            //var stock = _context.Stock.ToList();// will return a list of Stock Entity class

            var stock = await _stockRepository.GetAll(queryObject);
             var st =   stock.Select(x => x.ToStockDto()); // now it will return an array of StockDto class
             return Ok(st);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockDetailsById([FromRoute]int id) 
        { 
            //var stock = await _context.Stock.FindAsync(id);
            var stockIdKey = "stock" + id;
            var stock = await _redisCacheService.GetDataAsync<Stock>(stockIdKey);
            if (stock == null)
            {
                 stock = await _stockRepository.GetStockDetailsById(id);
                 await _redisCacheService.SetDataAsync(stockIdKey, stock);

            }


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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var  _stockModel = CretaStockDtoToModel.CreateStockDtoToModelMaper(createStockDto);
            var check = await _stockRepository.CreateStock(_stockModel);
            if (check == null)
            {
                return BadRequest("Stock Not created due to some errorS");
            }

            return Ok(createStockDto);
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpadeStock([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.StockId == id);
            var stockModel =  await _stockRepository.GetStockDetailsById(id);
            if (stockModel == null)
            {
                return NotFound($"Did not found object with object Id {id}");
            }
            var stock = await _stockRepository.UpdateStock(stockModel,updateStockDto );
           // stockModel.Symbol = updateStockDto.Symbol;
           // stockModel.MarketCap = updateStockDto.MarketCap;
           // stockModel.Purchase = updateStockDto.Purchase;
           // stockModel.CompanyName = updateStockDto.CompanyName;
           // stockModel.LastDiv  = updateStockDto.LastDiv;
           // stockModel.Industry = updateStockDto.Industry;
           //await _context.SaveChangesAsync();
           if (stock == null) return BadRequest("Not updated Correctly");

            return Ok(updateStockDto);
        }

        [HttpDelete]
        [Route("{id:int}")] // Note : There should not be space between Id:int
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            //var stock = await _context.Stock.FirstOrDefaultAsync(x => x.StockId == id);
            var stock = await _stockRepository.GetStockDetailsById(id); 
            if (stock == null) return NotFound();

            var isDelete = await _stockRepository.DeleteStock(stock);
            //_context.Stock.Remove(stock);
            //await _context.SaveChangesAsync();
            if (!isDelete) return BadRequest();
            return NoContent();
        }
    }
}
