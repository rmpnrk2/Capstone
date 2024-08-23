using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("invoices")][ApiController] public class InvoiceController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public InvoiceController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Invoice> invoices = await _dataContext.Invoices.
                Include(invoice => invoice.Items).
                AsNoTracking().ToListAsync();

            return Results.Ok(invoices);
        }

        [HttpGet("{invoiceID}")] public async Task<IResult> GetAsync(int invoiceID)
        {
            Invoice? invoice = await _dataContext.Invoices.Where(invoice => invoice.ID == invoiceID)
                .Include(invoice => invoice.Items)
                .FirstOrDefaultAsync();

            if(invoice == null)
                return Results.NotFound();

            return Results.Ok(invoice);
        }

        [HttpPut("{invoiceID}")] public async Task<IResult> PutAsync(int invoiceID, Invoice updatedInvoice)
        {
            int rowsAffected = await _dataContext.Invoices.Where(invoice => invoice.ID == invoiceID).ExecuteUpdateAsync(updates => updates
                .SetProperty(invoice => invoice.Title, updatedInvoice.Title)
                .SetProperty(invoice => invoice.CompanyName, updatedInvoice.CompanyName)
                .SetProperty(invoice => invoice.CompanyAddress, updatedInvoice.CompanyAddress)
                .SetProperty(invoice => invoice.CompanyZIPCode, updatedInvoice.CompanyZIPCode)
                .SetProperty(invoice => invoice.CompanyPhone, updatedInvoice.CompanyPhone)
                .SetProperty(invoice => invoice.AccountName, updatedInvoice.AccountName)
                .SetProperty(invoice => invoice.AccountAddress, updatedInvoice.AccountAddress)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        [HttpDelete("{invoiceID}")] public async Task<IResult> DeleteAsync(int invoiceID)
        {
            var rowsAffected = await _dataContext.Invoices.Where(invoice => invoice.ID == invoiceID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
