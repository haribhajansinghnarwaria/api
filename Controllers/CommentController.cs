using api.DTOMappers;
using api.DTOs.Comment;
using api.DTOs.Stocks;
using api.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentAsync()
        {
            var list = await _commentRepository.GetAllCommentAsync();
            var listOfCommentDto = list.Select(x => x.CommentModelToDto());

            return Ok(listOfCommentDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            var result = await _commentRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result.CommentModelToDto());
        }
        [HttpPost("{StockId:int}")]// this StockId should be exactly same as of what we are passing in parametrs even casing should be excatly same.
        public async Task<IActionResult> CreateCommentAsync([FromRoute] int StockId,CreateCommentDto createCommentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(! await _stockRepository.IsStockExist(StockId))
            {
                return BadRequest("Stock doesn't exist");
            }
            var commentModel = createCommentDto.ToCommentModelfromCreatedCommentDto(StockId);

            await _commentRepository.CreateCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetCommentById), new { id = commentModel }, commentModel.CommentModelToDto());
        }

        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> UpdateCommentByIdAsync([FromRoute] int commentId ,UpdateCommentDto updateCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _commentRepository.UpdateCommentByIdAsync(commentId, updateCommentDto.ToCommentModelfromUpdateCommentDto());
            if (result == null) { NotFound("Comment doesn't exist"); }
            return Ok(result.CommentModelToDto());
        }
    }
}
