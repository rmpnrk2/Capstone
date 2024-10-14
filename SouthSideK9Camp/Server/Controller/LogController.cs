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

        // Get
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Shared.Log> logs = await _dataContext.Logs
                .AsNoTracking().ToListAsync();

            return Results.Ok(logs);
        }

        [HttpGet("{logID}")] public async Task<IResult> GetAsync(int logID)
        {
            Shared.Log? log = await _dataContext.Logs.FirstOrDefaultAsync(log => log.ID == logID);

            if(log == null)
                return Results.NotFound();

            return Results.Ok(log);
        }
    }
}
