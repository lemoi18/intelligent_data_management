using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Site.Data;
using System.Linq;

namespace Site.Controllers
{
    public class PostgresController : Controller
    {
        private readonly ILogger<PostgresController> _logger;
        private readonly ApplicationDbContext _dbContext; // For PostgreSQL

        public PostgresController(ILogger<PostgresController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> SalesData()
        {
            try
            {
                var data = await _dbContext.Sales.OrderByDescending(s => s.InvoiceNo).Take(10).ToListAsync(); // Use the correct property name
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching sales data: {Error}", ex);
                return View("Error");
            }
        }

    }
}