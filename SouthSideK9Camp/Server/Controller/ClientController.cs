using BlazorTemplater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Server.Services;
using SouthSideK9Camp.Shared;

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
                    .ThenInclude(dog => dog.ProgressReports)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Invoices)
                        .ThenInclude(invoices => invoices.Items)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Reservation)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Contract)
                .AsNoTracking().ToListAsync();

            var json = JsonConvert.SerializeObject(clients, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<List<Shared.Client>>(json));
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
                    .ThenInclude(dog => dog.ProgressReports)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Invoices)
                        .ThenInclude(invoices => invoices.Items)
                .Include(client => client.Dogs)
                    .ThenInclude(dog => dog.Reservation)
                .FirstOrDefaultAsync();

            var json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Client>(json));
        }

        // check email availability for member
        [HttpGet("email/{email}")] public async Task<IResult> GetMemberEmailAvailabilityAsync(string email)
        {
            Shared.Client? client = await _dataContext.Clients.FirstOrDefaultAsync(c => c.Email == email);

            if (client == null) return Results.NotFound();

            var json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Client>(json));
        }

        // get client using email
        [HttpGet("check-member-email-availability/{email}")] public async Task<IResult> GetByEmailAsync(string email)
        {
            List<string> clientEmails = await _dataContext.Clients.Where(m => m.Member != null).Select(m => m.Email).ToListAsync();

            foreach(string clientEmail in clientEmails)
            {
                if(email == clientEmail) return Results.Conflict("Email already exist");
            }

            return Results.Ok("Email is available");
        }

        // membership registration
        [HttpPost("member-registration")] public async Task<IResult> MembershipRegistrationAsync(Shared.Client client)
        {
            // if client already exists
            Shared.Client? existingClient = await _dataContext.Clients.FindAsync(client.ID);
            if(existingClient == null)
            {
                _dataContext.Clients.Add(client);
            }
            else
            {
                existingClient.Member = client.Member;
            }

            await _dataContext.SaveChangesAsync();

            // send email
            string emailSubject = "SouthSide K9 Camp Membership Registration";
            string emailBody = new ComponentRenderer<EmailTemplates.MembershipRegistrationTemplate>()
                .Set(c => c.clientName, client.FirstName + " " + client.LastName)
                .Set(c => c.client_guid, client.Member?.GUID.ToString())
                .Set(c => c.host, _configuration["Host"])
                .Render();
            await _smtp.SendEmailAsync(client.Email, emailSubject, emailBody);

            var json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Shared.Client? deserializedClient = JsonConvert.DeserializeObject<Shared.Client>(json);
            return Results.CreatedAtRoute("GetClient", new {clientID = client.ID}, deserializedClient);
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