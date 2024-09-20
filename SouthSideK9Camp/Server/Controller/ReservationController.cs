using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("reservations")][ApiController] public class ReservationController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ReservationController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // getall
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Reservation> reservations = await _dataContext.Reservations
                .Include(r => r.Dogs)
                    .ThenInclude(d => d.Client)
                        .ThenInclude(c => c.Customer)
                .Include(r => r.Dogs)
                    .ThenInclude(d => d.Contract)
                .Include(r => r.Dogs)
                    .ThenInclude(d => d.ProgressReports)
                .Include(r => r.Dogs)
                    .ThenInclude(d => d.Invoices)
                        .ThenInclude(i => i.Items)
                .AsNoTracking().ToListAsync();

            string json = JsonConvert.SerializeObject(reservations, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<List<Shared.Reservation>>(json));
        }

        // get by id
        [HttpGet("{reservationID}", Name = "GetReservation")] public async Task<IResult> GetAsync(int reservationID)
        {
            Reservation? reservation = await _dataContext.Reservations.Where(reservation => reservation.ID == reservationID).FirstOrDefaultAsync();

            if(reservation == null)
                return Results.NotFound();

            string json = JsonConvert.SerializeObject(reservation, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Reservation>(json));
        }

        // get by guid
        [HttpGet("guid/{reservationGUID}")] public async Task<IResult> GetByGUIDAsync(string reservationGUID)
        {

            Shared.Reservation? reservation = await _dataContext.Reservations
                .Include(r => r.Dogs)
                    .ThenInclude(dogs => dogs.Client)
                .FirstOrDefaultAsync(r => r.GUID.ToString() == reservationGUID);

            if (reservation == null)
                return Results.NotFound();

            string json = JsonConvert.SerializeObject(reservation, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<Shared.Reservation>(json));
        }

        // add
        [HttpPost()] public async Task<IResult> PostAsync(Reservation reservation)
        {
            await _dataContext.AddAsync(reservation);
            await _dataContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetReservation", new {reservationID = reservation.ID}, reservation);
        }

        // update
        [HttpPut()] public async Task<IResult> PutAsync(Reservation updatedReservation)
        {
            int rowsAffected = await _dataContext.Reservations.Where(reservation => reservation.ID == updatedReservation.ID).ExecuteUpdateAsync(updates => updates
                .SetProperty(user => user.Name, updatedReservation.Name)
                .SetProperty(user => user.StartingDate, updatedReservation.StartingDate)
                .SetProperty(user => user.EndingDate, updatedReservation.EndingDate)
                .SetProperty(user => user.Slots, updatedReservation.Slots)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        // delete
        [HttpDelete("{reservationID}")] public async Task<IResult> DeleteAsync(int reservationID)
        {
            var rowsAffected = await _dataContext.Reservations.Where(reservation => reservation.ID == reservationID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
