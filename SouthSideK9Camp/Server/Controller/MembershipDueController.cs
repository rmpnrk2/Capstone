using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("membership-dues")][ApiController] public class MembershipDueController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public MembershipDueController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<MembershipDue> membershipDues = await _dataContext.MembershipDues.AsNoTracking().ToListAsync();

            return Results.Ok(membershipDues);
        }

        [HttpGet("{membershipDueID}")] public async Task<IResult> GetAsync(int membershipDueID)
        {
            MembershipDue? membershipDue = await _dataContext.MembershipDues.Where(membershipDue => membershipDue.ID == membershipDueID).FirstOrDefaultAsync();

            if (membershipDue == null)
                return Results.NotFound();

            return Results.Ok(membershipDue);
        }

        [HttpDelete("{membershipDueID}")] public async Task<IResult> DeleteAsync(int membershipDueID)
        {
            var rowsAffected = await _dataContext.MembershipDues.Where(membershipDue => membershipDue.ID == membershipDueID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
