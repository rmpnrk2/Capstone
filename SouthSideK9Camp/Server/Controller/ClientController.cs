using BlazorTemplater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Server.Services;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("clients")][ApiController] public class ClientController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IEmailService _smtp;
        private readonly IConfiguration _configuration;

        public ClientController(DataContext dataContext, IEmailService emailService, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _smtp = emailService;
            _configuration = configuration;
        }

        // get all
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Shared.Client> clients = await _dataContext.Clients
                .Include(client => client.Customer)
                .Include(client => client.Member)
                    .ThenInclude(member => member.MembershipDues)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Contract)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.ProgressReport)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Invoices)
                        .ThenInclude(invoices => invoices.Items)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Reservation)
                .AsNoTracking().ToListAsync();

            return Results.Ok(clients);
        }

        // get by id
        [HttpGet("{clientID}", Name = "GetClient")] public async Task<IResult> GetAsync(int clientID)
        {
            Shared.Client? client = await _dataContext.Clients.Where(client => client.ID == clientID)
                .Include(client => client.Customer)
                .Include(client => client.Member)
                    .ThenInclude(member => member.MembershipDues)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Contract)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.ProgressReport)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Invoices)
                        .ThenInclude(invoices => invoices.Items)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Reservation)
                .FirstOrDefaultAsync();

            if(client == null)
                return Results.NotFound();

            return Results.Ok(client);
        }

        // check email availability for customers
        [HttpGet("check-customer-email-availability/{email}")] public async Task<IResult> GetCustomerEmailAvailabilityAsync(string email)
        {
            List<string> clientEmails = await _dataContext.Clients.Where(m => m.Customer != null).Select(m => m.Email).ToListAsync();

            foreach(string clientEmail in clientEmails)
            {
                if(email == clientEmail) return Results.Conflict("Email already exist");
            }

            return Results.Ok("Email is available");
        }

        // check email availability for member
        [HttpGet("check-member-email-availability/{email}")] public async Task<IResult> GetMemberEmailAvailabilityAsync(string email)
        {
            List<string> clientEmails = await _dataContext.Clients.Where(m => m.Member != null).Select(m => m.Email).ToListAsync();

            foreach(string clientEmail in clientEmails)
            {
                if(email == clientEmail) return Results.Conflict("Email already exist");
            }

            return Results.Ok("Email is available");
        }

        // customer registration
        [HttpPost("customer-registration")] public async Task<IResult> CustomerRegistrationAsync(Shared.Client client)
        {
            _dataContext.Clients.Add(client);
            await _dataContext.SaveChangesAsync();
            return Results.CreatedAtRoute("GetClient", new {clientID = client.ID}, client);
        }

        // membership registration
        [HttpPost("member-registration")] public async Task<IResult> MembershipRegistrationAsync(Shared.Client client)
        {
            _dataContext.Clients.Add(client);
            await _dataContext.SaveChangesAsync();

            // send email
            string emailSubject = "SouthSide K9 Camp Membership Registration";
            string emailBody = new ComponentRenderer<EmailTemplates.MembershipRegistrationTemplate>()
                .Set(c => c.client_guid, client.Member.GUID.ToString())
                .Set(c => c.host, _configuration["Host"])
                .Render();
            await _smtp.SendEmailAsync(client.Email, emailSubject, emailBody);

            return Results.CreatedAtRoute("GetClient", new {clientID = client.ID}, client);
        }

        // TODO: customer registration with existing email
        [HttpPost("customer-registration-existing-email")] public async Task<IResult> CustomerRegistrationWithExistingEmailAsync(Shared.Client client)
        {
            _dataContext.Clients.Add(client);
            await _dataContext.SaveChangesAsync();
            return Results.CreatedAtRoute("GetClient", new {clientID = client.ID}, client);
        }

        // TODO: membership registration with existing email
        [HttpPost("member-registration-existing-email")] public async Task<IResult> MemberRegistrationWithExistingEmailAsync(Shared.Client client)
        {
            _dataContext.Clients.Add(client);
            await _dataContext.SaveChangesAsync();
            return Results.CreatedAtRoute("GetClient", new {clientID = client.ID}, client);
        }

        // update
        [HttpPut("{clientID}")] public async Task<IResult> PutAsync(int clientID, Shared.Client updatedClient)
        {
            int rowsAffected = await _dataContext.Clients.Where(client => client.ID == clientID).ExecuteUpdateAsync(updates => updates
                .SetProperty(client => client.LastName, updatedClient.LastName)
                .SetProperty(client => client.FirstName, updatedClient.FirstName)
                .SetProperty(client => client.MiddleInitial, updatedClient.MiddleInitial)
                .SetProperty(client => client.Sex, updatedClient.Sex)
                .SetProperty(client => client.Email, updatedClient.Email)
                .SetProperty(client => client.Contact, updatedClient.Contact)
                .SetProperty(client => client.Address, updatedClient.Address)
                .SetProperty(client => client.Birthday, updatedClient.Birthday)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
        
        // delete
        [HttpDelete("{clientID}")] public async Task<IResult> DeleteAsync(int clientID)
        {
            var rowsAffected = await _dataContext.Clients.Where(client => client.ID == clientID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}