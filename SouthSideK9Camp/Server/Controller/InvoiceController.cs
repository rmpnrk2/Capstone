using BlazorTemplater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Server.Services;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("invoice")][ApiController] public class InvoiceController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _smtp;

        public InvoiceController(DataContext dataContext, IConfiguration configuration, IEmailService emailService, IWebHostEnvironment hostingEnvironment)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _smtp = emailService;
            _hostingEnvironment = hostingEnvironment;
        }

        // get
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Invoice> invoices = await _dataContext.Invoices.
                Include(invoice => invoice.Items).
                AsNoTracking().ToListAsync();

            return Results.Ok(invoices);
        }

        // get by id
        [HttpGet("{invoiceID}", Name = "GetInvoice")] public async Task<IResult> GetAsync(int invoiceID)
        {
            Invoice? invoice = await _dataContext.Invoices.Where(invoice => invoice.ID == invoiceID)
                .Include(invoice => invoice.Items)
                .FirstOrDefaultAsync();

            if(invoice == null)
                return Results.NotFound();

            return Results.Ok(invoice);
        }

        // get default
        [HttpGet("default")] public async Task<IResult> GetDefaultAsync()
        {
            Invoice? invoice = await _dataContext.Invoices.FirstOrDefaultAsync(i => i.isDefault == true);

            if(invoice == null)
                return Results.NotFound();

            return Results.Ok(invoice);
        }

        // get by guid
        [HttpGet("guid/{invoiceGUID}")] public async Task<IResult> GetAsync(string invoiceGUID)
        {
            Shared.Invoice? invoice = await _dataContext.Invoices
                .Include(i => i.Items)
                .Include(i => i.Dog)
                    .ThenInclude(d => d.Client)
                .FirstOrDefaultAsync(i => i.GUID.ToString() == invoiceGUID);

            if (invoice == null) return Results.NotFound();

            string json = JsonConvert.SerializeObject(invoice, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Invoice>(json));
        }

        // create
        [HttpPost("{dogID}")] public async Task<IResult> PostAsync(int dogID, Shared.Invoice invoice)
        {
            invoice.DogID = dogID;
            invoice.ID = 0;
            _dataContext.Invoices.Add(invoice);
            await _dataContext.SaveChangesAsync();

            string json = JsonConvert.SerializeObject(invoice, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Shared.Invoice deserializedInvoice = JsonConvert.DeserializeObject<Shared.Invoice>(json)!;
            return Results.CreatedAtRoute("GetInvoice", new {invoiceID = invoice.ID}, deserializedInvoice);
        }

        // create default 
        [HttpPost("default")] public async Task<IResult> PostDefaultAsync(Shared.Invoice invoice)
        {
            // check if default already exist
            await _dataContext.Invoices.Where(i => i.isDefault).ExecuteUpdateAsync(updates => updates
                .SetProperty(i => i.isDefault, false));

            invoice.isDefault = true;
            _dataContext.Invoices.Add(invoice);
            await _dataContext.SaveChangesAsync();

            string json = JsonConvert.SerializeObject(invoice, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Shared.Invoice deserializedInvoice = JsonConvert.DeserializeObject<Shared.Invoice>(json)!;
            return Results.CreatedAtRoute("GetInvoice", new {invoiceID = invoice.ID}, deserializedInvoice);
        }

        // update
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

        // invoice payment
        [HttpPut("payment/{invoiceID}")] public async Task<IResult> Payment(int invoiceID, IFormFile imageContent)
        {
            // create unique GUID for image file
            string imageFileName = $"{Guid.NewGuid()}{Extension()}";
            string Extension()
            {
                if (imageContent.ContentType == "image/jpeg") return ".jpeg";
                if (imageContent.ContentType == "image/png") return ".png";
                return string.Empty;
            }

            // save image to wwwroot/Images/InvoicePayment
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Images/InvoicePayments", imageFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageContent.CopyToAsync(stream);
            }

            // set invoice to paid
            await _dataContext.Invoices.Where(i => i.ID == invoiceID).ExecuteUpdateAsync(updates => updates
                .SetProperty(i => i.ProofOfPaymentURL, _configuration["Host"] + "/Images/InvoicePayments/" + imageFileName)
            );

            await _dataContext.SaveChangesAsync();

            return Results.Ok();
        }

        // invoice payment resubmit
        [HttpPut("payment-resubmit/{invoiceID}")] public async Task<IResult> PaymentResubmit(int invoiceID)
        {
            // remove payment submission
            int rowsUpdated = await _dataContext.Invoices.Where(i => i.ID == invoiceID).ExecuteUpdateAsync(updates => updates
                .SetProperty(i => i.ProofOfPaymentURL, string.Empty)
            );

            return (rowsUpdated == 0) ? Results.NotFound() : Results.NoContent();
        }

        // invoice payment approve
        [HttpPut("payment-approve/{invoiceID}")] public async Task<IResult> ApprovePaymentAsync(int invoiceID)
        {
            // remove payment submission
            int rowsUpdated = await _dataContext.Invoices.Where(i => i.ID == invoiceID).ExecuteUpdateAsync(updates => updates
                .SetProperty(i => i.PaymentConfirmed, true)
            );

            return (rowsUpdated == 0) ? Results.NotFound() : Results.NoContent();
        }

        // invoice payment reject
        [HttpPut("payment-reject/{invoiceID}")] public async Task<IResult> RejectPaymentAsync(int invoiceID, Shared.ReasonForRejection reason)
        {
            // find invoice
             Shared.Invoice? invoice = await _dataContext.Invoices
                .Include(i => i.Items)
                .Include(i => i.Dog)
                    .ThenInclude(d => d.Client)
                .FirstOrDefaultAsync(i => i.ID == invoiceID);

            if (invoice == null) return Results.NotFound();

            // create reason for rejection
            _dataContext.Reasons.Add(reason);
            await _dataContext.SaveChangesAsync();

            // remove payment
            invoice.ProofOfPaymentURL = string.Empty;
            await _dataContext.SaveChangesAsync();

            // send rejection email
            string emailSubject = "SouthSide K9 Camp Statement of Account Payment Unsuccesssul";
            string emailBody = new ComponentRenderer<EmailTemplates.InvoicePaymentRejection>()
                .Set(c => c.invoice, invoice)
                .Set(c => c.host, _configuration["Host"])
                .Set(c => c.reason, reason)
                .Render();
            await _smtp.SendEmailAsync(invoice.Dog?.Client?.Email ?? string.Empty, emailSubject, emailBody);

            return Results.NoContent();
        }

        // send email to client
        [HttpPut("sendemail/{invoiceID}")] public async Task<IResult> PutAsync(int invoiceID)
        {
            Shared.Invoice? invoice = await _dataContext.Invoices
                .Include(i => i.Items)
                .Include(i => i.Dog)
                    .ThenInclude(d => d.Client)
                .FirstOrDefaultAsync(i => i.ID == invoiceID);

            if (invoice == null) return Results.NotFound();

            // update invoice status
            invoice.IsEmailed = true;
            invoice.DateSent = DateTime.UtcNow.AddHours(8);
            invoice.DateSent = DateTime.UtcNow.AddDays(30).AddHours(8);
            await _dataContext.SaveChangesAsync();
            
            // email to client
            string emailSubject = "SouthSide K9 Camp Statement of Account";
            string emailBody = new ComponentRenderer<EmailTemplates.InvoicePayment>()
                .Set(c => c.invoice, invoice)
                .Set(c => c.host, _configuration["Host"])
                .Render();
            await _smtp.SendEmailAsync(invoice.Dog?.Client?.Email ?? string.Empty, emailSubject, emailBody);

            return Results.NoContent();
        }

        // delete
        [HttpDelete("{invoiceID}")] public async Task<IResult> DeleteAsync(int invoiceID)
        {
            var rowsAffected = await _dataContext.Invoices.Where(invoice => invoice.ID == invoiceID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
