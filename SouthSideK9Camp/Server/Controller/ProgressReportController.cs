using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("progress-reports")][ApiController] public class ProgressReportController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProgressReportController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<ProgressReport> progressReport = await _dataContext.ProgressReports.AsNoTracking().ToListAsync();

            return Results.Ok(progressReport);
        }

        [HttpGet("{progressReportID}")] public async Task<IResult> GetAsync(int progressReportID)
        {
            ProgressReport? progressReport = await _dataContext.ProgressReports.Where(progressReport => progressReport.ID == progressReportID).FirstOrDefaultAsync();

            if(progressReport == null)
                return Results.NotFound();

            return Results.Ok(progressReport);
        }

        [HttpPut("{progressReportID}")] public async Task<IResult> PutAsync(int progressReportID, ProgressReport updatedProgressReport)
        {
            int rowsAffected = await _dataContext.ProgressReports.Where(progressReport => progressReport.ID == progressReportID).ExecuteUpdateAsync(updates => updates
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

        [HttpDelete("{progressReportID}")] public async Task<IResult> DeleteAsync(int progressReportID)
        {
            var rowsAffected = await _dataContext.ProgressReports.Where(progressReport => progressReport.ID == progressReportID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
