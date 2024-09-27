using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("reason-for-rejection")][ApiController] public class ReasonForRejectionController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ReasonForRejectionController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // get by id
        [HttpGet("{reasonID}", Name = "GetReason")] public async Task<IResult> GetAsync(int reasonID)
        {
            Shared.ReasonForRejection? reason = await _dataContext.Reasons.FirstOrDefaultAsync(r => r.ID == reasonID);

            return (reason != null) ? Results.Ok(reason) : Results.NotFound()  ;
        }

        // get by guid
        [HttpGet("guid/{reasonGUID}")] public async Task<IResult> GetByGUIDAsync(string reasonGUID)
        {
            Shared.ReasonForRejection? reason = await _dataContext.Reasons.FirstOrDefaultAsync(d => d.GUID.ToString() == reasonGUID);

            return (reason != null) ? Results.Ok(reason) : Results.NotFound()  ;
        }

        // create
        [HttpPost()] public async Task<IResult> PostAsync(Shared.ReasonForRejection reason)
        {
            _dataContext.Reasons.Add(reason);
            await _dataContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetReason", new {reasonID = reason.ID}, reason);
        }

        // delete
        [HttpDelete("{reasonID}")] public async Task<IResult> DeleteAsync(int reasonID)
        {
            var rowsAffected = await _dataContext.Reasons.Where(client => client.ID == reasonID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
