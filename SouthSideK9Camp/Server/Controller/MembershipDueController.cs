﻿using BlazorTemplater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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


            var json = JsonConvert.SerializeObject(membershipDues, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.MembershipDue>(json));
        }

        // get
        [HttpGet("{membershipDueID}")] public async Task<IResult> GetAsync(int membershipDueID)
        {
            MembershipDue? membershipDue = await _dataContext.MembershipDues.Where(membershipDue => membershipDue.ID == membershipDueID).FirstOrDefaultAsync();

            if (membershipDue == null)
                return Results.NotFound();

            var json = JsonConvert.SerializeObject(membershipDue, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.MembershipDue>(json));
        }

        // get by guid
        [HttpGet("guid/{membershipDueGUID}")] public async Task<IResult> GetAsync(string membershipDueGUID)
        {
            Shared.Client? client = await _dataContext.Clients
                .Where(c => c.Member != null && c.Member.MembershipDues.Any(d => d.GUID.ToString() == membershipDueGUID))
                .Include(c => c.Member).ThenInclude(m => m!.MembershipDues).FirstOrDefaultAsync();

            if (client == null)
                return Results.NotFound();

            var json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Client>(json));
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

            // Create new log for Membership due payment
            Shared.MembershipDue mebershipDue = _dataContext.MembershipDues
                .Include(m => m.Member).ThenInclude(m => m.Client)
                .FirstOrDefault(m => m.ID == membershipDueID) ?? new();

            Shared.Log log = new()
            {
              Message = "Membership Due Payment",
              Subject = mebershipDue.Member?.Client?.FirstName + " " + mebershipDue.Member?.Client?.MiddleInitial + " " + mebershipDue.Member?.Client?.LastName,
              Severity = "Severity.Info"
            };

            _dataContext.Logs.Add(log);
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
            // find membership due
            Shared.MembershipDue? membershipDue = await _dataContext.MembershipDues
                .Include(m => m.Member).ThenInclude(m => m.Client)
                .FirstOrDefaultAsync(d => d.ID == membershipDueID);

            if(membershipDue == null)
                return Results.NotFound();
            
            // set membership due to paid
            membershipDue.PaymentConfirmed = true;
            await _dataContext.SaveChangesAsync();

            // create new membershp due
            Shared.MembershipDue newMembershipDue = new()
            {
                MemberID = membershipDueID,
            };
            _dataContext.MembershipDues.Add(newMembershipDue);

            // Create new receipt
            Shared.Receipt receipt = new()
            {
                receiptType = 4,
                Balance = 1500,
            };
            _dataContext.Receipts.Add(receipt);
            await _dataContext.SaveChangesAsync();
            membershipDue.ReceiptID = receipt.ID;
            await _dataContext.SaveChangesAsync();

            // email client
            string emailSubject = "SouthSide K9 Camp Membership Due Payment Successful";
            string emailBody = new ComponentRenderer<EmailTemplates.PaymentConfirmationMembershipDue>()
                .Set(c => c.membershipDue, membershipDue)
                .Set(c => c.client, membershipDue.Member?.Client)
                .Render();

            if(membershipDue.Member != null && membershipDue.Member.Client != null)
                await _smtp.SendEmailAsync(membershipDue.Member.Client.Email, emailSubject, emailBody);

            return Results.NoContent();
        }

        // membership due payment reject
        [HttpPut("payment-reject/{membershipDueID}")] public async Task<IResult> RejectDuePaymentAsync(int membershipDueID, Shared.ReasonForRejection reason)
        {
            // find membership
            Shared.MembershipDue? membershipDue = await _dataContext.MembershipDues
                .Include(d => d.Member).ThenInclude(m => m!.Client)
                .FirstOrDefaultAsync(d => d.ID == membershipDueID);

            if(membershipDue == null) return Results.NotFound();

            // create reason for rejection
            _dataContext.Reasons.Add(reason);
            await _dataContext.SaveChangesAsync();

            // remove payment
            membershipDue.ProofOfPaymentURL = string.Empty;

            await _dataContext.SaveChangesAsync();

            // send payment rejection email
            string emailSubject = "SouthSide K9 Camp Membership Due Payment Unsuccessful";
            string emailBody = new ComponentRenderer<EmailTemplates.MembershipDuePaymentRejection>()
                .Set(c => c.clientName, membershipDue.Member?.Client?.FirstName + " " + membershipDue.Member?.Client?.LastName)
                .Set(c => c.membershipDueGUID, membershipDue.GUID.ToString())
                .Set(c => c.host, _configuration["Host"])
                .Set(c => c.dueDate, membershipDue.DateTimeDue)
                .Set(c => c.reason, reason)
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

            // filter members and accept those who are about to expire
            List<Shared.MembershipDue> membershipDuesAboutToExpire = membershipDues
                .Where(d => d.DateTimeDue.AddDays(-5).Day == DateTime.UtcNow.Day)
                .Where(d => d.DateTimeDue.Month == DateTime.UtcNow.Month)
                .Where(d => d.DateTimeDue.Year == DateTime.UtcNow.Year)
                .Where(d => d.ProofOfPaymentURL == string.Empty)
                .Where(d => !d.PaymentConfirmed)
                .ToList();

            foreach(Shared.MembershipDue due in membershipDuesAboutToExpire)
            {
                string emailSubject = "SouthSide K9 Camp Membership Due is About to Expire";
                string emailBody = new ComponentRenderer<EmailTemplates.MembershipDuePaymentReminder5DaysBefore>()
                    .Set(c => c.clientName, due.Member?.Client?.FirstName ?? string.Empty + " " + due.Member?.Client?.LastName ?? string.Empty)
                    .Set(c => c.membershipDueGUID, due.GUID.ToString())
                    .Set(c => c.host, _configuration["Host"])
                    .Set(c => c.dueDate, due.DateTimeDue)
                    .Render();
                await _smtp.SendEmailAsync(due.Member?.Client?.Email ?? string.Empty, emailSubject, emailBody);
                    
                count += 1;
            }

            // filter members and accept those who just expired
            List<Shared.MembershipDue> membershipDuesJustExpired = membershipDues
                .Where(d => d.DateTimeDue.Day == DateTime.UtcNow.Day)
                .Where(d => d.DateTimeDue.Month == DateTime.UtcNow.Month)
                .Where(d => d.DateTimeDue.Year == DateTime.UtcNow.Year)
                .Where(d => d.ProofOfPaymentURL == string.Empty)
                .Where(d => !d.PaymentConfirmed)
                .ToList();

            foreach(Shared.MembershipDue due in membershipDuesJustExpired)
            {
                string emailSubject = "SouthSide K9 Camp Membership Due";
                string emailBody = new ComponentRenderer<EmailTemplates.MembershipDuePaymentReminder>()
                    .Set(c => c.clientName, due.Member?.Client?.FirstName ?? string.Empty + " " + due.Member?.Client?.LastName ?? string.Empty)
                    .Set(c => c.membershipDueGUID, due.GUID.ToString())
                    .Set(c => c.host, _configuration["Host"])
                    .Set(c => c.dueDate, due.DateTimeDue)
                    .Render();
                await _smtp.SendEmailAsync(due.Member?.Client?.Email ?? string.Empty, emailSubject, emailBody);
                    
                count += 1;
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
