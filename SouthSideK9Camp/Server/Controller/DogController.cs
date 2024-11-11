using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("dog")][ApiController] public class DogController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public DogController(DataContext dataContext, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _environment = environment;
            _configuration = configuration;
        }

        // get all
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Dog> dogs = await _dataContext.Dogs
                .Include(dog => dog.Client)
                .Include(dog => dog.Reservation)
                .Include(dog => dog.Contract)
                .Include(dog => dog.ProgressReports)
                .Include(dog => dog.Invoices)
                    .ThenInclude(invoice => invoice.Items)
                .AsNoTracking().ToListAsync();

            string json = JsonConvert.SerializeObject(dogs, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<List<Shared.Dog>>(json));
        }

        // get by id
        [HttpGet("{dogID}")] public async Task<IResult> GetAsync(int dogID)
        {
            Dog? dog = await _dataContext.Dogs.Where(dog => dog.ID == dogID)
                .Include(dog => dog.Client)
                .Include(dog => dog.Reservation)
                .Include(dog => dog.Contract)
                .Include(dog => dog.ProgressReports)
                .Include(dog => dog.Invoices)
                    .ThenInclude(invoice => invoice.Items)
                .FirstOrDefaultAsync();

            if(dog == null)
                return Results.NotFound();

            string json = JsonConvert.SerializeObject(dog, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Dog>(json));
        }

        // get by guid
        [HttpGet("guid/{dogGUID}")] public async Task<IResult> GetByGUIDAsync(string dogGUID)
        {

            Shared.Dog? dog = await _dataContext.Dogs
                .Include(c => c.Client)
                .Include(c => c.Reservation)
                .Include(dog => dog.Contract)
                .Include(dog => dog.ProgressReports)
                .FirstOrDefaultAsync(d => d.GUID.ToString() == dogGUID);

            if (dog == null)
                return Results.NotFound();

            var json = JsonConvert.SerializeObject(dog, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Dog>(json));
        }

        // update
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

        // upload avatar
        [HttpPost("upload-avatar/{dogID}")] public async Task<IResult> RegistrationPayment(int dogID, IFormFile imageContent)
        {
            // create unique GUID for image file
            string imageFileName = $"{Guid.NewGuid()}{Extension()}";
            string Extension()
            {
                if (imageContent.ContentType == "image/jpeg") return ".jpeg";
                if (imageContent.ContentType == "image/png") return ".png";
                return string.Empty;
            }

            // save image to wwwroot/Images/DogAvatar
            string path = Path.Combine(_environment.WebRootPath, "Images/DogAvatar", imageFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageContent.CopyToAsync(stream);
            }

            int rowsAffected = await _dataContext.Dogs.Where(dog => dog.ID == dogID).ExecuteUpdateAsync(updates => updates
                .SetProperty(d => d.AvatarURL, _configuration["Host"] + "/Images/DogAvatar/" + imageFileName));

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        // delete
        [HttpDelete("{dogID}")] public async Task<IResult> DeleteAsync(int dogID)
        {
            var rowsAffected = await _dataContext.Dogs.Where(dog => dog.ID == dogID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        // Delete dogs who are expired
        [HttpDelete("delete-expired")] public async Task<IResult> DeleteExpiredAsync()
        {
            List<Shared.Dog> dogs = _dataContext.Dogs.Include(d => d.Reservation).AsNoTracking().ToList();

            int rowsDeleted = 0;

            foreach (Shared.Dog dog in dogs)
            {
                DateTime? deadLine = (dog.DateCreated.AddDays(5) > dog.Reservation?.StartingDate) ? dog.Reservation.StartingDate : dog.DateCreated.AddDays(5);
                TimeSpan? timeLeft = deadLine - DateTime.UtcNow;

                if (timeLeft != null)
                    if ((int)timeLeft.Value.TotalDays <= 0)
                    {
                        await _dataContext.Dogs.Where(d => d.ID == dog.ID).ExecuteDeleteAsync();
                        rowsDeleted++;
                    }
            }
            return rowsDeleted == 0 ? Results.NotFound() : Results.NoContent();
        }

    }
}
