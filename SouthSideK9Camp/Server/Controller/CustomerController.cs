using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("customers")][ApiController] public class CustomerController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CustomerController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // get all
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Customer> customers = await _dataContext.Customers.AsNoTracking().ToListAsync();

            return Results.Ok(customers);
        }

        // get single
        [HttpGet("{customerID}")] public async Task<IResult> GetAsync(int customerID)
        {
            Customer? customer = await _dataContext.Customers.Where(customer => customer.ID == customerID).FirstOrDefaultAsync();

            if(customer == null)
                return Results.NotFound();

            return Results.Ok(customer);
        }

        // update
        [HttpPut("{customerID}")] public async Task<IResult> PutAsync(int customerID, Customer updatedCustomer)
        {
            int rowsAffected = await _dataContext.Customers.Where(customer => customer.ID == customerID).ExecuteUpdateAsync(updates => updates
                .SetProperty(customer => customer.WhereWillYouBeStating, updatedCustomer.WhereWillYouBeStating)
                .SetProperty(customer => customer.EmergencyVet, updatedCustomer.EmergencyVet)
                .SetProperty(customer => customer.EmergencyVetNumber, updatedCustomer.EmergencyVetNumber)
                .SetProperty(customer => customer.EmergencyContactName, updatedCustomer.EmergencyContactName)
                .SetProperty(customer => customer.EmergencyContactNumber, updatedCustomer.EmergencyContactNumber)
                .SetProperty(customer => customer.EmergencyContactEmail, updatedCustomer.EmergencyContactEmail)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        // delete
        [HttpDelete("{customerID}")] public async Task<IResult> DeleteAsync(int customerID)
        {
            var rowsAffected = await _dataContext.Customers.Where(customer => customer.ID == customerID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
