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
        [HttpPut("{reportID}")] public async Task<IResult> PutAsync(int reportID, ProgressReport updatedProgressReport)
        {
            int rowsAffected = await _dataContext.ProgressReports.Where(progressReport => progressReport.ID == reportID).ExecuteUpdateAsync(updates => updates
                .SetProperty(progressReport => progressReport.Subject, updatedProgressReport.Subject)
                .SetProperty(progressReport => progressReport.Message, updatedProgressReport.Message)
                .SetProperty(progressReport => progressReport.DateTraining, updatedProgressReport.DateTraining)
                .SetProperty(progressReport => progressReport.SpanDuration, updatedProgressReport.SpanDuration)
                .SetProperty(progressReport => progressReport.ScoreFocus, updatedProgressReport.ScoreFocus)
                .SetProperty(progressReport => progressReport.ScoreObedience, updatedProgressReport.ScoreObedience)
                .SetProperty(progressReport => progressReport.ScoreProtection, updatedProgressReport.ScoreProtection)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        // delete
        [HttpDelete("{reportID}")] public async Task<IResult> DeleteAsync(int reportID)
        {
            var rowsAffected = await _dataContext.ProgressReports.Where(progressReport => progressReport.ID == reportID).ExecuteDeleteAsync();
            return (rowsAffected) == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
