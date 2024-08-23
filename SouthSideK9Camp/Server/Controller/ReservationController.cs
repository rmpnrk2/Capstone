using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Reservation> reservations = await _dataContext.Reservations
                .Include(reservation => reservation.Dogs)
                    .ThenInclude(dogs => dogs.Client)
                .AsNoTracking().ToListAsync();

            return Results.Ok(reservations);
        }

        [HttpGet("{reservationID}")] public async Task<IResult> GetAsync(int reservationID)
        {
            Reservation? reservation = await _dataContext.Reservations.Where(reservation => reservation.ID == reservationID).FirstOrDefaultAsync();

            if(reservation == null)
                return Results.NotFound();

            return Results.Ok(reservation);
        }

        [HttpPut("{reservationID}")] public async Task<IResult> PutAsync(int reservationID, Reservation updatedReservation)
        {
            int rowsAffected = await _dataContext.Reservations.Where(reservation => reservation.ID == reservationID).ExecuteUpdateAsync(updates => updates
                .SetProperty(user => user.Name, updatedReservation.Name)
                .SetProperty(user => user.StartingDate, updatedReservation.StartingDate)
                .SetProperty(user => user.EndingDate, updatedReservation.EndingDate)
                .SetProperty(user => user.Slots, updatedReservation.Slots)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        [HttpDelete("{reservationID}")] public async Task<IResult> DeleteAsync(int reservationID)
        {
            var rowsAffected = await _dataContext.Reservations.Where(reservation => reservation.ID == reservationID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
