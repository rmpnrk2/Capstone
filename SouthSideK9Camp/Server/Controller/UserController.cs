using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("users")][ApiController] public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<User> users = await _dataContext.Users.AsNoTracking().ToListAsync();

            return Results.Ok(users);
        }

        [HttpGet("{userID}")] public async Task<IResult> GetAsync(int userID)
        {
            User? user = await _dataContext.Users.Where(user => user.ID == userID).FirstOrDefaultAsync();

            if(user == null)
                return Results.NotFound();

            return Results.Ok(user);
        }

        [HttpPut("{userID}")] public async Task<IResult> PutAsync(int userID, User updatedUser)
        {
            int rowsAffected = await _dataContext.Users.Where(invoice => invoice.ID == userID).ExecuteUpdateAsync(updates => updates
                .SetProperty(user => user.Username, updatedUser.Username)
                .SetProperty(user => user.Password, updatedUser.Password)
                .SetProperty(user => user.Email, updatedUser.Email)
                .SetProperty(user => user.FullName, updatedUser.FullName)
                .SetProperty(user => user.Contact, updatedUser.Contact)
                .SetProperty(user => user.Role, updatedUser.Role)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        [HttpDelete("{userID}")] public async Task<IResult> DeleteAsync(int userID)
        {
            var rowsAffected = await _dataContext.Users.Where(user => user.ID == userID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
