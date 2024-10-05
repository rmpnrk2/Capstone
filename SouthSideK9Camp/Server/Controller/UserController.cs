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

        // Get all users
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<User> users = await _dataContext.Users.AsNoTracking().ToListAsync();

            return Results.Ok(users);
        }

        // Get users using UserID
        [HttpGet("{userID}", Name = "GetUser")] public async Task<IResult> GetAsync(int userID)
        {
            User? user = await _dataContext.Users.Where(user => user.ID == userID).FirstOrDefaultAsync();

            if(user == null)
                return Results.NotFound();

            return Results.Ok(user);
        }

        // Check login credentials
        [HttpGet("login/{username}/{password}")] public async Task<IResult> LoginAsync(string username, string password)
        {
            User? user = await _dataContext.Users.Where(user => user.Username == username && user.Password == password).FirstOrDefaultAsync();

            if(user == null)
                return Results.NotFound();

            return Results.Ok(user);
        }

        // Create use
        [HttpPost()] public async Task<IResult> PutAsync(User user)
        {
            // Check for conflict, there should be no User.Duplicate
            Shared.User? existingUser = _dataContext.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null) return Results.Conflict();

            existingUser = _dataContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null) return Results.Conflict();
            
            // Creates new user if there is no conflict
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetUser", new {userID = user.ID}, user);
        }

        // Update user
        [HttpPut("{userID}")] public async Task<IResult> PutAsync(int userID, User updatedUser)
        {
            // Check for conflict, there should be no User.Duplicate. However if the updatedUser is the same as the existingUser then it's okay if User.Username is the same
            Shared.User? existingUser = _dataContext.Users.FirstOrDefault(u => u.Username == updatedUser.Username);
            if (existingUser != null && updatedUser.ID != existingUser.ID) return Results.Conflict();

            existingUser = _dataContext.Users.FirstOrDefault(u => u.Email == updatedUser.Email);
            if (existingUser != null && updatedUser.ID != existingUser.ID) return Results.Conflict();
            
            // Update user if there is no conflict
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

        // Delete user
        [HttpDelete("{userID}")] public async Task<IResult> DeleteAsync(int userID)
        {
            var rowsAffected = await _dataContext.Users.Where(user => user.ID == userID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

    }
}
