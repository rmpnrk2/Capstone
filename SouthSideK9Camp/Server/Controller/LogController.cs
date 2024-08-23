using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("logs")] [ApiController] public class LogController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public LogController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Shared.Log> logs = await _dataContext.Logs
                .Include(log => log.Subject)
                .Include(log => log.Message)
                .Include(log => log.Severity)
                .AsNoTracking().ToListAsync();

            return Results.Ok(logs);
        }

        [HttpGet("{logID}")] public async Task<IResult> GetAsync(int logID)
        {
            Shared.Log? log = await _dataContext.Logs.Where(log => log.ID == logID)
                .Include(log => log.Subject)
                .Include(log => log.Message)
                .Include(log => log.Severity)
                .FirstOrDefaultAsync();

            if(log == null)
                return Results.NotFound();

            return Results.Ok(log);
        }

        [HttpPut("{logID}")] public async Task<IResult> PutAsync(int logID, Shared.Log updatedLog)
        {
            int rowsAffected = await _dataContext.Logs.Where(log => log.ID == logID).ExecuteUpdateAsync(updates => updates
                .SetProperty(log => log.Subject, updatedLog.Subject)
                .SetProperty(log => log.Message, updatedLog.Message)
                .SetProperty(log => log.Severity, updatedLog.Severity)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        [HttpDelete("{logID}")] public async Task<IResult> DeleteAsync(int logID)
        {
            var rowsAffected = await _dataContext.Logs.Where(log => log.ID == logID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
