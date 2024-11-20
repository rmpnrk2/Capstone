using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SouthSideK9Camp.Server.Data;

namespace SouthSideK9Camp.Server.Controller
{
    [Route("receipt")] [ApiController] public class ReceiptController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ReceiptController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET all
        [HttpGet()] public async Task<IResult> GetAsync()
        {
            List<Shared.Receipt> receipts = await _dataContext.Receipts
                .Include(r => r.Dog).ThenInclude(d => d.Client)
                .Include(r => r.Invoice).ThenInclude(d => d.Dog).ThenInclude(d => d.Client)
                .Include(r => r.Member).ThenInclude(d => d.Client)
                .Include(r => r.MembershipDue).ThenInclude(d => d.Member).ThenInclude(d => d.Client)
                .AsNoTracking().ToListAsync();

            var json = JsonConvert.SerializeObject(receipts, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Results.Ok(JsonConvert.DeserializeObject<List<Shared.Receipt>>(json));
        }
    }
}
