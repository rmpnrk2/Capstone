using BlazorTemplater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Server.Services;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("membership-dues")][ApiController] public class MembershipDueController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _smtp;

        public MembershipDueController(DataContext dataContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEmailService emailService)
        {
            _dataContext = dataContext;
            _hostingEnvironment = webHostEnvironment;
            _configuration = configuration;
            _smtp = emailService;
        }

        // getall
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<MembershipDue> membershipDues = await _dataContext.MembershipDues.Include(d => d.Member).AsNoTracking().ToListAsync();

            return Results.Ok(membershipDues);
        }

        // get
        [HttpGet("{membershipDueID}")] public async Task<IResult> GetAsync(int membershipDueID)
        {
            MembershipDue? membershipDue = await _dataContext.MembershipDues.Where(membershipDue => membershipDue.ID == membershipDueID).FirstOrDefaultAsync();

            if (membershipDue == null)
                return Results.NotFound();

            return Results.Ok(membershipDue);
        }

        // get by guid
        [HttpGet("guid/{membershipDueGUID}")] public async Task<IResult> GetAsync(string membershipDueGUID)
        {
            Shared.Client? client = await _dataContext.Clients
                .Where(c => c.Member != null && c.Member.MembershipDues.Any(d => d.GUID.ToString() == membershipDueGUID))
                .Include(c => c.Member).ThenInclude(m => m!.MembershipDues).FirstOrDefaultAsync();

            if (client == null)
                return Results.NotFound();

            return Results.Ok(client);
        }

        // membership due payment
        [HttpPost("payment/{membershipDueID}")] public async Task<IResult> MembershipDuePayment(int membershipDueID, IFormFile imageContent)
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
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Images/MembershipDuePayments", imageFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageContent.CopyToAsync(stream);
            }

            // set member to paid
            await _dataContext.MembershipDues.Where(due => due.ID == membershipDueID).ExecuteUpdateAsync(updates => updates
                .SetProperty(due => due.ProofOfPaymentURL, _configuration["Host"] + "/Images/MembershipDuePayments/" + imageFileName)
            );

            await _dataContext.SaveChangesAsync();

            return Results.Ok();
        }

        // membership due payment resubmit
        [HttpPut("payment-resubmit/{membershipDueID}")] public async Task<IResult> MembershipDuePaymentResubmit(int membershipDueID)
        {
            // remove payment submission
            Shared.MembershipDue? membershipDue = await _dataContext.MembershipDues.FirstOrDefaultAsync(d => d.ID == membershipDueID);

            if(membershipDue == null)
                return Results.NotFound();
            
            membershipDue.ProofOfPaymentURL = string.Empty;
            await _dataContext.SaveChangesAsync();

            return Results.Ok();
        }

        // membership due payment approve
        [HttpPut("payment-approve/{membershipDueID}")] public async Task<IResult> ApproveDuePaymentAsync(int membershipDueID)
        {
            Shared.MembershipDue? membershipDue = await _dataContext.MembershipDues.FirstOrDefaultAsync(d => d.ID == membershipDueID);

            if(membershipDue == null)
                return Results.NotFound();
            
            membershipDue.PaymentConfirmed = true;
            await _dataContext.SaveChangesAsync();

            return Results.NoContent();
        }

        // membership due payment reject
        [HttpPut("payment-reject/{membershipDueID}")] public async Task<IResult> RejectDuePaymentAsync(int membershipDueID)
        {
            Shared.MembershipDue? membershipDue = await _dataContext.MembershipDues
                .Include(d => d.Member).ThenInclude(m => m!.Client)
                .FirstOrDefaultAsync(d => d.ID == membershipDueID);

            if(membershipDue == null)
                return Results.NotFound();

            membershipDue.ProofOfPaymentURL = string.Empty;

            await _dataContext.SaveChangesAsync();

            // send payment rejection email
            string emailSubject = "SouthSide K9 Camp Membership Due";
            string emailBody = new ComponentRenderer<EmailTemplates.MembershipDuePaymentRejection>()
                .Set(c => c.membershipDueGUID, membershipDue.GUID.ToString())
                .Set(c => c.host, _configuration["Host"])
                .Set(c => c.dueDate, membershipDue.DateTimeDue)
                .Render();
            await _smtp.SendEmailAsync(membershipDue.Member?.Client?.Email ?? string.Empty, emailSubject, emailBody);

            return Results.NoContent();

        }

        // membership due payment notify
        [HttpPut("payment-notify")] public async Task<IResult> DuePaymentNotifyAsync()
        {
            List<Shared.MembershipDue> membershipDues = await _dataContext.MembershipDues.Include(d => d.Member).ThenInclude(m => m.Client).ToListAsync();

            if(membershipDues.Count == 0)
                return Results.NotFound();

            // send payment rejection email
            int count = 0;

            foreach(Shared.MembershipDue due in membershipDues)
            {
                if(!due.PaymentConfirmed && due.ProofOfPaymentURL == string.Empty && due.DateTimeDue < DateTime.UtcNow)
                {
                    string emailSubject = "SouthSide K9 Camp Membership Due";
                    string emailBody = new ComponentRenderer<EmailTemplates.MembershipDuePaymentReminder>()
                        .Set(c => c.membershipDueGUID, due.GUID.ToString())
                        .Set(c => c.host, _configuration["Host"])
                        .Set(c => c.dueDate, due.DateTimeDue)
                        .Render();
                    await _smtp.SendEmailAsync(due.Member.Client.Email, emailSubject, emailBody);
                    
                    count += 1;
                }
            }
            return Results.Ok(count);

        }

        // delete
        [HttpDelete("{membershipDueID}")] public async Task<IResult> DeleteAsync(int membershipDueID)
        {
            var rowsAffected = await _dataContext.MembershipDues.Where(membershipDue => membershipDue.ID == membershipDueID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
