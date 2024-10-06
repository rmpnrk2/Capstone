using BlazorTemplater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Server.Services;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("members")][ApiController] public class MemberController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _smtp;

        public MemberController(DataContext dataContext, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IEmailService emailService)
        {
            _dataContext = dataContext;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _smtp = emailService;
        }

        // get all
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Member> members = await _dataContext.Members
                .Include(member => member.MembershipDues)
                .AsNoTracking().ToListAsync();

             return Results.Ok(members);
        }

        // get single
        [HttpGet("{memberID}")] public async Task<IResult> GetAsync(int memberID)
        {
            Member? member = await _dataContext.Members.Where(member => member.ID == memberID).Include(
                member => member.MembershipDues).FirstOrDefaultAsync();

            if (member == null)
                return Results.NotFound();

            return Results.Ok(member);
        }

        // get by guid
        [HttpGet("guid/{memberGUID}")] public async Task<IResult> GetAsync(string memberGUID)
        {

            Shared.Client? client = await _dataContext.Clients.Where(c => c.Member != null && c.Member.GUID.ToString() == memberGUID)
                .Include(c => c.Member).FirstOrDefaultAsync();

            if (client == null)
                return Results.NotFound();

            return Results.Ok(client);
        }

        // registration payment
        [HttpPost("registration-payment/{memberID}")] public async Task<IResult> RegistrationPayment(int memberID, IFormFile imageContent)
        {
            // create unique GUID for image file
            string imageFileName = $"{Guid.NewGuid()}{Extension()}";
            string Extension()
            {
                if (imageContent.ContentType == "image/jpeg") return ".jpeg";
                if (imageContent.ContentType == "image/png") return ".png";
                return string.Empty;
            }

            // save image to wwwroot/Images/MembershipRegistrationPayment
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Images/MembershipRegistrationPayment", imageFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageContent.CopyToAsync(stream);
            }

            // set member to paid
            await _dataContext.Members.Where(member => member.ID == memberID).ExecuteUpdateAsync(updates => updates
                .SetProperty(member => member.RegistrationPaymentURL, _configuration["Host"] + "/Images/MembershipRegistrationPayment/" + imageFileName)
            );

            await _dataContext.SaveChangesAsync();

            return Results.Ok();
        }

        // registration payment resubmit
        [HttpPost("registration-payment-resubmit/{memberID}")] public async Task<IResult> RegistrationPaymentResubmit(int memberID)
        {
            // remove payment submission
            await _dataContext.Members.Where(member => member.ID == memberID).ExecuteUpdateAsync(updates => updates
                .SetProperty(member => member.RegistrationPaymentURL, string.Empty)
            );

            await _dataContext.SaveChangesAsync();

            return Results.Ok();
        }

        // update
        [HttpPut("{clientID}")] public async Task<IResult> EditAsync(int clientID, Shared.Client updatedClient)
        {
            var rowsAffected = await _dataContext.Clients.Where(c => c.ID == updatedClient.ID).ExecuteUpdateAsync(updates => updates
                .SetProperty(c => c.LastName, updatedClient.LastName)
                .SetProperty(c => c.FirstName, updatedClient.FirstName)
                .SetProperty(c => c.MiddleInitial, updatedClient.MiddleInitial)
                .SetProperty(c => c.Sex, updatedClient.Sex)
                .SetProperty(c => c.Email, updatedClient.Email)
                .SetProperty(c => c.Contact, updatedClient.Contact)
                .SetProperty(c => c.Address, updatedClient.Address)
                .SetProperty(c => c.Birthday, updatedClient.Birthday)
            );

            var client = await _dataContext.Clients.Include(c => c.Member).FirstOrDefaultAsync(c => c.ID == updatedClient.ID);
            if (client != null)
                if(client.Member != null && updatedClient.Member != null)
                {
                    client.Member.Occupation = updatedClient.Member.Occupation;
                    client.Member.WhereDidYouHereAboutUs = updatedClient.Member.WhereDidYouHereAboutUs;
                    client.Member.PurposeOfJoining = updatedClient.Member.PurposeOfJoining;
                    client.Member.DogClinicAddress = updatedClient.Member.DogClinicAddress;
                    client.Member.WhoTrainYourDog = updatedClient.Member.WhoTrainYourDog;
                    await _dataContext.SaveChangesAsync();
                }
            
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        // approve membership payment
        [HttpPut("payment-approve/{clientID}")] public async Task<IResult> ApproveAsync(int clientID)
        {
            // find client
            var client = await _dataContext.Clients
                .Include(c => c.Member).ThenInclude(m => m.MembershipDues)
                .FirstOrDefaultAsync(c => c.ID == clientID);

            if (client == null || client.Member == null)
                return Results.NotFound();

            client.Member.RegistrationConfirmed = true;

            // create membership due
            client.Member.MembershipDues.Add(new());
            await _dataContext.SaveChangesAsync();

            // email client
            string emailSubject = "SouthSideK9 Camp Board & Train Registration Payment Unsuccessful";
            string emailBody = new ComponentRenderer<EmailTemplates.PaymentConfirmationMembershipDue>()
                .Set(c => c.client, client)
                .Render();
            await _smtp.SendEmailAsync(client.Email, emailSubject, emailBody);

            return Results.NoContent();
        }

        // reject membership payment
        [HttpPut("payment-reject/{clientID}")] public async Task<IResult> RejectAsync(int clientID, Shared.ReasonForRejection reason)
        {
            var client = await _dataContext.Clients.Include(c => c.Member).FirstOrDefaultAsync(c => c.ID == clientID);

            if (client == null || client.Member == null)
                    return Results.NotFound();

            // create reason for rejection
            _dataContext.Reasons.Add(reason);
            await _dataContext.SaveChangesAsync();

            // remove payment
            client.Member.RegistrationPaymentURL = string.Empty;
            await _dataContext.SaveChangesAsync();

            // send email
            string emailSubject = "SouthSideK9 Camp Membership Registration Payment Unsuccessful";
            string emailBody = new ComponentRenderer<EmailTemplates.MembershipRegistrationRejectedTemplate>()
                .Set(c => c.clientName, client.FirstName + " " + client.LastName)
                .Set(c => c.client_guid, client.Member.GUID.ToString())
                .Set(c => c.reason, reason)
                .Set(c => c.host, _configuration["Host"])
                .Render();
            await _smtp.SendEmailAsync(client.Email, emailSubject, emailBody);

            return Results.NoContent();
        }

        // delete
        [HttpDelete("{memberID}")] public async Task<IResult> DeleteAsync(int memberID)
        {
            var rowsAffected = await _dataContext.Members.Where(member => member.ID == memberID).ExecuteDeleteAsync();

            // if delete if member is null
            await _dataContext.Clients.Where(c => c.Customer == null && c.Member == null).ExecuteDeleteAsync();

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

    }
}