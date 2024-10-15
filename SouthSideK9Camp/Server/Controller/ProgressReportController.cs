using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("report")][ApiController] public class ProgressReportController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public ProgressReportController(DataContext dataContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _hostingEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        // create
        [HttpPost("{dogID}")] public async Task<IResult> GetAsync(int dogID, [FromForm] string reportContent, IFormFile? imageContent)
        {
            Shared.ProgressReport report = JsonConvert.DeserializeObject<Shared.ProgressReport>(reportContent) ?? new();
            report.DogID = dogID;

            if (imageContent != null)
            {
                // create unique GUID for image file
                string imageFileName = $"{Guid.NewGuid()}{Extension()}";
                string Extension()
                {
                    if (imageContent.ContentType == "image/jpeg") return ".jpeg";
                    if (imageContent.ContentType == "image/png") return ".png";
                    return string.Empty;
                }

                // save image to wwwroot/Images/ReportImages
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "Images/ReportImages", imageFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await imageContent.CopyToAsync(stream);
                }

                report.ImageURL = _configuration["Host"] + "/Images/ReportImages/" + imageFileName;
            }

            _dataContext.ProgressReports.Add(report);
            _dataContext.SaveChanges();

            return Results.Ok();
        }

        // update
        [HttpPut("{reportID}")] public async Task<IResult> PutAsync(int reportID, [FromForm] string reportContent, IFormFile? imageContent)
        {
            Shared.ProgressReport? report = _dataContext.ProgressReports.FirstOrDefault(r => r.ID == reportID);
            Shared.ProgressReport reportDesrialized = JsonConvert.DeserializeObject<Shared.ProgressReport>(reportContent) ?? new();

            if (report != null)
            {
                report.Subject = reportDesrialized.Subject;
                report.Message = reportDesrialized.Message;
                report.DateCreated = reportDesrialized.DateCreated;
                report.SpanDuration = reportDesrialized.SpanDuration;
                report.ScoreFocus = reportDesrialized.ScoreFocus;
                report.ScoreObedience = reportDesrialized.ScoreObedience;
                report.ScoreProtection = reportDesrialized.ScoreProtection;

                if (imageContent != null) 
                {
                    // create unique GUID for image file
                    string imageFileName = $"{Guid.NewGuid()}{Extension()}";
                    string Extension()
                    {
                        if (imageContent.ContentType == "image/jpeg") return ".jpeg";
                        if (imageContent.ContentType == "image/png") return ".png";
                        return string.Empty;
                    }

                    // save image to wwwroot/Images/ReportImages
                    string path = Path.Combine(_hostingEnvironment.WebRootPath, "Images/ReportImages", imageFileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await imageContent.CopyToAsync(stream);
                    }

                    report.ImageURL = _configuration["Host"] + "/Images/ReportImages/" + imageFileName;
                }
                _dataContext.SaveChanges();

                return Results.NoContent();
            }
            return Results.NotFound();
        }

        // delete
        [HttpDelete("{reportID}")] public async Task<IResult> DeleteAsync(int reportID)
        {
            var rowsAffected = await _dataContext.ProgressReports.Where(progressReport => progressReport.ID == reportID).ExecuteDeleteAsync();
            return (rowsAffected) == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
