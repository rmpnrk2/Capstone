using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("items")][ApiController] public class ItemController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ItemController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Item> items = await _dataContext.Items.AsNoTracking().ToListAsync();

            return Results.Ok(items);
        }

        [HttpGet("{itemID}")] public async Task<IResult> GetAsync(int itemID)
        {
            Item? item = await _dataContext.Items.Where(item => item.ID == itemID).FirstOrDefaultAsync();

            if(item == null)
                return Results.NotFound();

            return Results.Ok(item);
        }

        [HttpPut("{itemID}")] public async Task<IResult> PutAsync(int itemID, Item updatedItem)
        {
            int rowsAffected = await _dataContext.Items.Where(item => item.ID == itemID).ExecuteUpdateAsync(updates => updates
                .SetProperty(item => item.Name, updatedItem.Name)
                .SetProperty(item => item.MeasurementCount, updatedItem.MeasurementCount)
                .SetProperty(item => item.MeasurementUnit, updatedItem.MeasurementUnit)
                .SetProperty(item => item.Ammount, updatedItem.Ammount)
                .SetProperty(item => item.Price, updatedItem.Price)
                .SetProperty(item => item.Credit, updatedItem.Credit)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        [HttpDelete("{itemID}")] public async Task<IResult> DeleteAsync(int itemID)
        {
            var rowsAffected = await _dataContext.Items.Where(item => item.ID == itemID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
