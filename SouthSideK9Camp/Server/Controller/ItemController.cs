using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("item")][ApiController] public class ItemController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ItemController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // get
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Item> items = await _dataContext.Items.AsNoTracking().ToListAsync();

            return Results.Ok(items);
        }

        // get models
        [HttpGet("models")] public async Task<IResult> GetModelsAsync()
        {
            List<Item> items = await _dataContext.Items.Where(i => i.isModel).AsNoTracking().ToListAsync();

            return Results.Ok(items);
        }

        // get by id
        [HttpGet("{itemID}", Name = "GetItem")] public async Task<IResult> GetAsync(int itemID)
        {
            Item? item = await _dataContext.Items.Where(item => item.ID == itemID).FirstOrDefaultAsync();

            if(item == null)
                return Results.NotFound();

            return Results.Ok(item);
        }

        // add
        [HttpPost("{invoiceID}")] public async Task<IResult> PostAsync(int invoiceID, Shared.Item item)
        {
            item.ID = 0;
            item.isModel = false;
            item.InvoiceID = invoiceID;
            item.CalculateTotal();
            _dataContext.Items.Add(item);
            await _dataContext.SaveChangesAsync();


            var json = JsonConvert.SerializeObject(item, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Shared.Item deserializedItem = JsonConvert.DeserializeObject<Shared.Item>(json)!;
            return Results.CreatedAtRoute("GetItem", new {itemID = item.ID}, deserializedItem);
        }

        // add item model
        [HttpPost("model")] public async Task<IResult> PostModelAsync(Shared.Item item)
        {
            item.isModel = true;
            _dataContext.Items.Add(item);
            await _dataContext.SaveChangesAsync();

            var json = JsonConvert.SerializeObject(item, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Shared.Item deserializedItem = JsonConvert.DeserializeObject<Shared.Item>(json)!;
            return Results.CreatedAtRoute("GetItem", new {itemID = item.ID}, deserializedItem);
        }

        // update
        [HttpPut("{itemID}")] public async Task<IResult> PutAsync(int itemID, Item updatedItem)
        {
            Shared.Item? item = await _dataContext.Items.FirstOrDefaultAsync(i => i.ID == itemID);

            if (item == null) return Results.NotFound();

            item.Date = updatedItem.Date;
            item.Name = updatedItem.Name;
            item.Description = updatedItem.Description;
            item.Quantity = updatedItem.Quantity;
            item.Unit = updatedItem.Unit;
            item.Price = updatedItem.Price;
            item.Credit = updatedItem.Credit;

            // calculate item prive
            item.CalculateTotal();
            _dataContext.SaveChanges();

            // calculate invoice price
            Shared.Invoice? invoice = await _dataContext.Invoices.Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.ID == item.InvoiceID);

            if(invoice != null) invoice.CalculateBalance();
            _dataContext.SaveChanges();

            return Results.NoContent();
        }


        // delete
        [HttpDelete("{itemID}")] public async Task<IResult> DeleteAsync(int itemID)
        {
            var rowsAffected = await _dataContext.Items.Where(item => item.ID == itemID).ExecuteDeleteAsync();

            // calculate invoice price
            Shared.Invoice? invoice = await _dataContext.Invoices.Where(i => i.Items.Any(i => i.ID == itemID))
                .Include(i => i.Items).FirstOrDefaultAsync();

            if(invoice != null) invoice.CalculateBalance();
            _dataContext.SaveChanges();

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
