using BlazorTemplater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Server.Services;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("customer")][ApiController] public class CustomerController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _smtp;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CustomerController(DataContext dataContext, IConfiguration configuration, IEmailService emailService, IWebHostEnvironment hostEnvironment)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _smtp = emailService;
            _hostEnvironment = hostEnvironment;
        }

        // get all
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Customer> customers = await _dataContext.Customers.AsNoTracking().ToListAsync();

            return Results.Ok(customers);
        }

        // get by id
        [HttpGet("{customerID}", Name = "GetCustomer")] public async Task<IResult> GetAsync(int customerID)
        {
            Customer? customer = await _dataContext.Customers.Where(customer => customer.ID == customerID).FirstOrDefaultAsync();

            if(customer == null)
                return Results.NotFound();

            var json = JsonConvert.SerializeObject(customer, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Customer>(json));
        }

        // get by guid
        [HttpGet("guid/{clientGUID}")] public async Task<IResult> GetByGUIDAsync(string clientGUID)
        {

            Shared.Client? client = await _dataContext.Clients
                .Include(c => c.Customer)
                .Include(c => c.Dogs)
                .FirstOrDefaultAsync(c => c.GUID.ToString() == clientGUID);

            if (client == null)
                return Results.NotFound();

            var json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Client>(json));
        }

        // check for email availability
        [HttpGet("check-email/{email}")] public async Task<IResult> GetEmailAvailabilityAsync(string email)
        {
            Shared.Client? client = await _dataContext.Clients.FirstOrDefaultAsync(c => c.Email == email);

            if(client != null)
                return Results.Ok(client);
                
            return Results.NotFound();
        }

        // register
        [HttpPost("register/{reservationGUID}")] public async Task<IResult> MembershipRegistrationAsync(string reservationGUID, Shared.Client client)
        {
            Shared.Reservation? reservation = await _dataContext.Reservations.FirstOrDefaultAsync(r => r.GUID.ToString() == reservationGUID);

            // do not register dog if exceeded reservation limit
            if(reservation == null || (reservation.Slots - reservation.Dogs.Count) < client.Dogs.Count)
                return Results.NotFound();

            // Automatically adjust reservation ending date based on the training type duration
            foreach (Shared.Dog dog in client.Dogs)
            {
                int numberOfWeeks = (dog.Contract.TrainingType == "Basic Obedience (4 weeks)") ? 4 : 6; // Set number of weeks depending on the selected training type

                // If reservatin ending data is null set the ending date based on the number of weeks
                if (reservation.EndingDate == null)
                    reservation.EndingDate = reservation.StartingDate?.AddDays(numberOfWeeks * 7);

                // If reservation is set to 4 weeks then set to 6 weeks
                if (reservation.StartingDate?.AddDays(numberOfWeeks * 7) > reservation.EndingDate)
                    reservation.EndingDate = reservation.StartingDate?.AddDays(numberOfWeeks * 7);
            }

            // reserve dogs
            foreach(Shared.Dog dog in client.Dogs)
            {
                dog.ReservationID = reservation.ID;

                // send email for each registered dog
                string emailSubject = "SouthSideK9 Camp Board & Train Registration";
                string emailBody = new ComponentRenderer<EmailTemplates.CustomerRegistratinReservationPayment>()
                    .Set(c => c.clientName, client.FirstName + " " + client.LastName)
                    .Set(c => c.dogName, dog.Name)
                    .Set(c => c.startingDate, reservation.StartingDate)
                    .Set(c => c.endingDate, (reservation.DateCreated.AddDays(5) < reservation.EndingDate) ? reservation.DateCreated.AddDays(5) : reservation.EndingDate )
                    .Set(c => c.host, _configuration["Host"])
                    .Set(c => c.dogGUID, dog.GUID.ToString())
                    .Set(c => c.dueDate, (dog.DateCreated.AddDays(5) > reservation.StartingDate) ? reservation.StartingDate : dog.DateCreated.AddDays(5)) // Duedate is 5 days upon reservation or the ending date of reservation
                    .Render();
                await _smtp.SendEmailAsync(client.Email, emailSubject, emailBody);
            }

            // if client already exists
            Shared.Client? existingClient = await _dataContext.Clients.FindAsync(client.ID);
            if(existingClient == null)
            {
                _dataContext.Clients.Add(client);
            }
            else
            {
                existingClient.Customer = client.Customer;
                existingClient.Dogs = client.Dogs;
            }

            // Create new log for Board & Train registration
            Shared.Log log = new()
            {
              Message = "Board & Train Registration",
              Subject = client.FirstName + " " + client.MiddleInitial + " " + client.LastName,
              Severity = "Severity.Success"
            };

            _dataContext.Logs.Add(log);
            await _dataContext.SaveChangesAsync();


            // Return JSON with error loop handling
            var json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Shared.Client? deserializedClient = JsonConvert.DeserializeObject<Shared.Client>(json);

            return Results.CreatedAtRoute("GetCustomer", new {customerID = client.ID}, deserializedClient);
        }

        // reservation payment
        [HttpPost("reservation-payment/{dogID}")] public async Task<IResult> RegistrationPayment(int dogID, IFormFile imageContent)
        {
            // create unique GUID for image file
            string imageFileName = $"{Guid.NewGuid()}{Extension()}";
            string Extension()
            {
                if (imageContent.ContentType == "image/jpeg") return ".jpeg";
                if (imageContent.ContentType == "image/png") return ".png";
                return string.Empty;
            }

            // save image to wwwroot/Images/ReservationPayment
            string path = Path.Combine(_hostEnvironment.WebRootPath, "Images/ReservationPayment", imageFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageContent.CopyToAsync(stream);
            }

            // set dog reservation to paid
            await _dataContext.Dogs.Where(dog => dog.ID == dogID).ExecuteUpdateAsync(updates => updates
                .SetProperty(d => d.ReservationPaymentURL, _configuration["Host"] + "/Images/ReservationPayment/" + imageFileName));

            // Create new log for Board & Train registration payment
            Shared.Dog dog = _dataContext.Dogs.Include(d => d.Client).FirstOrDefault(d => d.ID == dogID) ?? new();

            Shared.Log log = new()
            {
              Message = "Board & Train Payment",
              Subject = dog.Client?.FirstName + " " + dog.Client?.MiddleInitial + " " + dog.Client?.LastName,
              Severity = "Severity.Success"
            };

            _dataContext.Logs.Add(log);
            await _dataContext.SaveChangesAsync();

            return Results.Ok();
        }

        // registration payment resubmit
        [HttpPost("reservation-payment-resubmit/{dogID}")] public async Task<IResult> RegistrationPaymentResubmit(int dogID)
        {
            // remove payment submission
            int rowsUpdated = await _dataContext.Dogs.Where(d => d.ID == dogID).ExecuteUpdateAsync(updates => updates
                .SetProperty(member => member.ReservationPaymentURL, string.Empty)
            );

            return (rowsUpdated == 0) ? Results.NotFound() : Results.NoContent();
        }

        // update
        [HttpPut("{customerID}")] public async Task<IResult> PutAsync(int customerID, Shared.Customer customerUpdate)
        {
            int rowsAffected = await _dataContext.Customers.Where(c => c.ID == customerID).ExecuteUpdateAsync(updates => updates
                .SetProperty(c => c.WhereWillYouBeStating, customerUpdate.WhereWillYouBeStating)
                .SetProperty(c => c.EmergencyVet, customerUpdate.EmergencyVet)
                .SetProperty(c => c.EmergencyVetNumber, customerUpdate.EmergencyVetNumber)
                .SetProperty(c => c.EmergencyContactName, customerUpdate.EmergencyContactName)
                .SetProperty(c => c.EmergencyContactNumber, customerUpdate.EmergencyContactNumber)
                .SetProperty(c => c.EmergencyContactEmail, customerUpdate.EmergencyContactEmail));

            return (rowsAffected == 0) ? Results.NotFound() : Results.NoContent();
        }

        // approve reservation payment
        [HttpPut("payment-approve/{dogID}")] public async Task<IResult> ApproveAsync(int dogID)
        {
            Shared.Dog? dog = await _dataContext.Dogs
                .Include(d => d.Client)
                .Include(d => d.Reservation)
                .FirstOrDefaultAsync(d => d.ID == dogID);
                
            if(dog == null) return Results.NotFound();

            dog.ReservationPaymentConfirmed = true;
            await _dataContext.SaveChangesAsync();

            // update dogs reservation duration
            Shared.Reservation? reservation = await _dataContext.Reservations.Include(r => r.Dogs).FirstOrDefaultAsync(r => r.ID == dogID);

            if(reservation != null)
            {
                if(reservation.Dogs.Any(d => d.Contract.TrainingType.Contains("Basic Obedience")))
                {
                    reservation.EndingDate = reservation.StartingDate?.AddDays(28);
                }
                if(reservation.Dogs.Any(d => d.Contract.TrainingType.Contains("Advanced Obedience")) || reservation.Dogs.Any(d => d.Contract.TrainingType.Contains("Behavioral Modification")) ||
                    reservation.Dogs.Any(d => d.Contract.TrainingType.Contains("Personal Protection Dog")) || reservation.Dogs.Any(d => d.Contract.TrainingType.Contains("Search and Rescue")))
                {
                    reservation.EndingDate = reservation.StartingDate?.AddDays(42);
                }
            }

            // email client
            string emailSubject = "SouthSideK9 Camp Board & Train Registration Payment Unsuccessful";
            string emailBody = new ComponentRenderer<EmailTemplates.PaymentConfirmationReservation>()
                .Set(c => c.client, dog.Client)
                .Set(c => c.reservation , dog.Reservation)
                .Set(c => c.dog, dog)
                .Render();

            if(dog.Client != null)
                await _smtp.SendEmailAsync(dog.Client.Email, emailSubject, emailBody);

            return Results.NoContent();
        }

        // reject reservation payment
        [HttpPut("payment-reject/{dogID}")] public async Task<IResult> RejectAsync(int dogID, Shared.ReasonForRejection reason)
        {
            var dog = await _dataContext.Dogs
                .Include(d => d.Client)
                .Include(d => d.Reservation)
                .FirstOrDefaultAsync(c => c.ID == dogID);

            if (dog == null || dog.Client == null)
                return Results.NotFound();

            // create reason for rejection
            _dataContext.Reasons.Add(reason);
            await _dataContext.SaveChangesAsync();

            // remove payment
            dog.ReservationPaymentURL = string.Empty;
            await _dataContext.SaveChangesAsync();

            // send payment rejection email
            string emailSubject = "SouthSideK9 Camp Board & Train Registration Payment Unsuccessful";
            string emailBody = new ComponentRenderer<EmailTemplates.CustomerRegistrationReservationRejection>()
                .Set(c => c.clientName, dog.Client.FirstName + " " + dog.Client.LastName)
                .Set(c => c.dogGUID, dog.GUID.ToString())
                .Set(c => c.reason, reason)
                .Set(c => c.host, _configuration["Host"])
                .Render();
            await _smtp.SendEmailAsync(dog.Client.Email, emailSubject, emailBody);

            return Results.NoContent();
        }

        // delete
        [HttpDelete("{customerID}")] public async Task<IResult> DeleteAsync(int customerID)
        {
            Shared.Customer? customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.ID == customerID);

            if(customer == null) return Results.NotFound();

            // also delete all the dogs
            await _dataContext.Dogs.Where(d => d.ClientID == customer.ClientID).ExecuteDeleteAsync();
            await _dataContext.Customers.Where(customer => customer.ID == customerID).ExecuteDeleteAsync();

            // if delete if member is null
            await _dataContext.Clients.Where(c => c.Customer == null && c.Member == null).ExecuteDeleteAsync();

            return Results.NoContent();
        }
    }
}
