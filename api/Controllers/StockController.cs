using api.Data;
using api.Dtos.Stocks;
using api.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository StockRepo;

    private readonly ApplicationDBContext Context;

    public StockController(ApplicationDBContext context, IStockRepository stockRepo)
    {
        Context = context;
       StockRepo = stockRepo;

    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await StockRepo.GetAllAsync();
         var stockDto = stocks.Select(s=> s.ToStockDto());
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await Context.Stocks.FindAsync(id);
        if(stock == null){
            return NotFound();
        }
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequest StockDto)
    {
        var stockModel =  StockDto.ToStockFromCreateDto();
        await Context.Stocks.AddAsync(stockModel);
        await Context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequest updateDto)
    {
        var stockModel = await Context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

        if(stockModel == null)
        {
            return NotFound();
        }

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry;
        stockModel.MarketCap = updateDto.MarketCap;

        await Context.SaveChangesAsync();

        return Ok(stockModel.ToStockDto());

    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await Context.Stocks.FirstOrDefaultAsync(x=> x.Id == id);
        if(stockModel == null)
        {
            return NotFound();
        }

        Context.Stocks.Remove(stockModel);

        await Context.SaveChangesAsync();

        return NoContent();
    }

}