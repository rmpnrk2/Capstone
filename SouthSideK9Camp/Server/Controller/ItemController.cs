using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;
using static MudBlazor.CategoryTypes;

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
            List<Shared.Item> items = await _dataContext.Items.AsNoTracking().ToListAsync();

            return Results.Ok(items);
        }

        // get models
        [HttpGet("models")] public async Task<IResult> GetModelsAsync()
        {
            List<Shared.Item> items = await _dataContext.Items.Where(i => i.isModel).AsNoTracking().ToListAsync();

            return Results.Ok(items);
        }

        // get by id
        [HttpGet("{itemID}", Name = "GetItem")] public async Task<IResult> GetAsync(int itemID)
        {
            Shared.Item? item = await _dataContext.Items.Where(item => item.ID == itemID).FirstOrDefaultAsync();

            if(item == null)
                return Results.NotFound();

            return Results.Ok(item);
        }

        // add
        [HttpPost("{invoiceID}")] public async Task<IResult> PostAsync(int invoiceID, Shared.Item item)
        {
            // add item
            item.ID = 0;
            item.isModel = false;
            item.InvoiceID = invoiceID;
            item.CalculateTotal();
            _dataContext.Items.Add(item);
            await _dataContext.SaveChangesAsync();

            // recalculate invoice price
            Shared.Invoice? invoice = await _dataContext.Invoices.Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.ID == item.InvoiceID);

            if(invoice != null) invoice.CalculateBalance();
            _dataContext.SaveChanges();

            // return result
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
        [HttpPut("{itemID}")] public async Task<IResult> PutAsync(int itemID, Shared.Item updatedItem)
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
            Shared.Invoice? invoice = await _dataContext.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.ID == item.InvoiceID);
            if(invoice != null) invoice.CalculateBalance();
            _dataContext.SaveChanges();

            return Results.NoContent();
        }


        // delete
        [HttpDelete("{itemID}")] public async Task<IResult> DeleteAsync(int itemID)
        {
            Shared.Item? item = await _dataContext.Items.FirstOrDefaultAsync(i => i.ID == itemID);

            if (item == null) return Results.NotFound();

            int? invoiceID = item.InvoiceID;

            _dataContext.Items.Remove(item);
            _dataContext.SaveChanges();
          
            // calculate invoice balance
            Shared.Invoice? invoice = await _dataContext.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.ID == invoiceID);
            if(invoice != null) invoice.CalculateBalance();
            _dataContext.SaveChanges();

            var json = JsonConvert.SerializeObject(invoice, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Shared.Invoice deserializedItem = JsonConvert.DeserializeObject<Shared.Invoice>(json)!;

            return Results.Ok(deserializedItem);
        }
    }
}
