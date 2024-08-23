using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthSideK9Camp.Server.Data;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("contracts")][ApiController] public class ContractController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ContractController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Contract> contracts = await _dataContext.Contracts.AsNoTracking().ToListAsync();

            return Results.Ok(contracts);
        }

        [HttpGet("{contractID}")] public async Task<IResult> GetAsync(int contractID)
        {
            Contract? contract = await _dataContext.Contracts.Where(contract => contract.ID == contractID).FirstOrDefaultAsync();

            if(contract == null)
                return Results.NotFound();

            return Results.Ok(contract);
        }

        [HttpPut("{contractID}")] public async Task<IResult> PutAsync(int contractID, Contract updatedContract)
        {
            int rowsAffected = await _dataContext.Contracts.Where(contract => contract.ID == contractID).ExecuteUpdateAsync(updates => updates
                .SetProperty(contract => contract.TrainingType, updatedContract.TrainingType)
                .SetProperty(contract => contract.TrainingGoal, updatedContract.TrainingGoal)
                .SetProperty(contract => contract.Restrictions, updatedContract.Restrictions)
                .SetProperty(contract => contract.FeedingRoutine, updatedContract.FeedingRoutine)
                .SetProperty(contract => contract.SleepingRoutine, updatedContract.SleepingRoutine)
                .SetProperty(contract => contract.ProtectiveOverToys, updatedContract.ProtectiveOverToys)
                .SetProperty(contract => contract.ProtectiveOverFood, updatedContract.ProtectiveOverFood)
                .SetProperty(contract => contract.ProtectiveOverTreats, updatedContract.ProtectiveOverTreats)
                .SetProperty(contract => contract.ProtectiveOverPerson, updatedContract.ProtectiveOverPerson)
                .SetProperty(contract => contract.ProtectiveOverOther, updatedContract.ProtectiveOverOther)
                .SetProperty(contract => contract.DiscomfortOverPaws, updatedContract.DiscomfortOverPaws)
                .SetProperty(contract => contract.DiscomfortOverTails, updatedContract.DiscomfortOverTails)
                .SetProperty(contract => contract.DiscomfortOverEars, updatedContract.DiscomfortOverEars)
                .SetProperty(contract => contract.DiscomfortOverMuzzle, updatedContract.DiscomfortOverMuzzle)
                .SetProperty(contract => contract.DiscomfortOverHead, updatedContract.DiscomfortOverHead)
                .SetProperty(contract => contract.DiscomfortOverOther, updatedContract.DiscomfortOverOther)
                .SetProperty(contract => contract.FearOrAggressionToHuman, updatedContract.FearOrAggressionToHuman)
                .SetProperty(contract => contract.FearOrAggressionToDogs, updatedContract.FearOrAggressionToDogs)
                .SetProperty(contract => contract.AnythingElseToShare, updatedContract.AnythingElseToShare)
            );

            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }

        [HttpDelete("{contractID}")] public async Task<IResult> DeleteAsync(int contractID)
        {
            var rowsAffected = await _dataContext.Contracts.Where(contract => contract.ID == contractID).ExecuteDeleteAsync();
            return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
}
