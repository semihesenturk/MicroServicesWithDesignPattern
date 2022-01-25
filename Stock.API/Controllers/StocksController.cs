using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        #region Variables
        private AppDbContext _context;
        #endregion

        #region Constructor
        public StocksController(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Actions
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Stocks.ToListAsync());
        }
        #endregion
    }
}
