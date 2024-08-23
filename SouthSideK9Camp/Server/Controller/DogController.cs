using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("dogs")][ApiController] public class DogController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public DogController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Dog> dogs = await _dataContext.Dogs
                .Include(dog => dog.Client)
                .Include(dog => dog.Reservation)
                .Include(dog => dog.Contract)
                .Include(dog => dog.ProgressReport)
                .Include(dog => dog.Invoices)
                    .ThenInclude(invoice => invoice.Items)
                .AsNoTracking().ToListAsync();

            return Results.Ok(dogs);
        }

        [HttpGet("{dogID}")] public async Task<IResult> GetAsync(int dogID)
        {
            Dog? dog = await _dataContext.Dogs.Where(dog => dog.ID == dogID)
                .Include(dog => dog.Client)
                .Include(dog => dog.Reservation)
                .Include(dog => dog.Contract)
                .Include(dog => dog.ProgressReport)
                .Include(dog => dog.Invoices)
                    .ThenInclude(invoice => invoice.Items)
                .FirstOrDefaultAsync();

            if(dog == null)
                return Results.NotFound();

            return Results.Ok(dog);
        }

        [HttpPut("{dogID}")] public async Task<IResult> PutAsync(int dogID, Dog updatedDog)
        {
            int rowsAffected = await _dataContext.Dogs.Where(dog => dog.ID == dogID).ExecuteUpdateAsync(updates => updates
                .SetProperty(dog => dog.Name, updatedDog.Name)
                .SetProperty(dog => dog.Breed, updatedDog.Breed)
                .SetProperty(dog => dog.Sex, updatedDog.Sex)
                .SetProperty(dog => dog.Birthday, updatedDog.Birthday)
                .SetProperty(dog => dog.AvatarURL, updatedDog.AvatarURL)
                .SetProperty(dog => dog.VaccineCardURL, updatedDog.VaccineCardURL)
                .SetProperty(dog => dog.Clinic, updatedDog.Clinic)
                .SetProperty(dog => dog.Rabies, updatedDog.Rabies)
                .SetProperty(dog => dog.Distemper, updatedDog.Distemper)
                .SetProperty(dog => dog.HepatitisAdenovirus, updatedDog.HepatitisAdenovirus)
                .SetProperty(dog => dog.Parvovirus, updatedDog.Parvovirus)
                .SetProperty(dog => dog.Parainfluenza, updatedDog.Parainfluenza)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        [HttpDelete("{dogID}")] public async Task<IResult> DeleteAsync(int dogID)
        {
            var rowsAffected = await _dataContext.Dogs.Where(dog => dog.ID == dogID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
