using BlazorTemplater;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("customer")][ApiController] public class CustomerController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CustomerController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // get all
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Customer> customers = await _dataContext.Customers.AsNoTracking().ToListAsync();

            return Results.Ok(customers);
        }

        // get single
        [HttpGet("{customerID}")] public async Task<IResult> GetAsync(int customerID)
        {
            Customer? customer = await _dataContext.Customers.Where(customer => customer.ID == customerID).FirstOrDefaultAsync();

            if(customer == null)
                return Results.NotFound();

            return Results.Ok(customer);
        }

        // get by guid
        [HttpGet("guid/{customerGUID}")] public async Task<IResult> GetByGUIDAsync(string customerGUID)
        {

            Shared.Client? client = await _dataContext.Clients.Where(c => c.Customer != null && c.Customer.GUID.ToString() == customerGUID)
                .Include(c => c.Customer).FirstOrDefaultAsync();

            if (client == null)
                return Results.NotFound();

            return Results.Ok(client);
        }

        // check for email availability
        [HttpGet("email-availability/{email}")] public async Task<IResult> GetEmailAvailabilityAsync(string email)
        {
            Shared.Client? client = await _dataContext.Clients.Where(m => m.Customer != null).FirstOrDefaultAsync(c => c.Email == email);

            if(client != null)
                return Results.Ok(client);
                
            return Results.NotFound();
        }

        // register
        //[HttpPost("register")] public async Task<IResult> MembershipRegistrationAsync(Shared.Client client)
        //{
        //    _dataContext.Clients.Add(client);
        //    await _dataContext.SaveChangesAsync();

        //    // send email
        //    string emailSubject = "SouthSide K9 Camp Membership Registration";
        //    string emailBody = new ComponentRenderer<EmailTemplates.MembershipRegistrationTemplate>()
        //        .Set(c => c.client_guid, client.Member.GUID.ToString())
        //        .Set(c => c.host, _configuration["Host"])
        //        .Render();
        //    await _smtp.SendEmailAsync(client.Email, emailSubject, emailBody);

        //    return Results.CreatedAtRoute("GetClient", new {clientID = client.ID}, client);
        //}

        // update
        [HttpPut("{customerID}")] public async Task<IResult> PutAsync(int customerID, Customer updatedCustomer)
        {
            int rowsAffected = await _dataContext.Customers.Where(customer => customer.ID == customerID).ExecuteUpdateAsync(updates => updates
                .SetProperty(customer => customer.WhereWillYouBeStating, updatedCustomer.WhereWillYouBeStating)
                .SetProperty(customer => customer.EmergencyVet, updatedCustomer.EmergencyVet)
                .SetProperty(customer => customer.EmergencyVetNumber, updatedCustomer.EmergencyVetNumber)
                .SetProperty(customer => customer.EmergencyContactName, updatedCustomer.EmergencyContactName)
                .SetProperty(customer => customer.EmergencyContactNumber, updatedCustomer.EmergencyContactNumber)
                .SetProperty(customer => customer.EmergencyContactEmail, updatedCustomer.EmergencyContactEmail)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        // delete
        [HttpDelete("{customerID}")] public async Task<IResult> DeleteAsync(int customerID)
        {
            var rowsAffected = await _dataContext.Customers.Where(customer => customer.ID == customerID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
