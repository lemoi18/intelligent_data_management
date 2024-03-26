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
        private readonly ApplicationDbContext _dbContext;

        public PostgresController(ILogger<PostgresController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult QueryOptions()
        {
            return View("QueryOptions");
        }

        public async Task<IActionResult> TotalSalesByCountry(int year)
{
    try
    {
        _logger.LogInformation("Fetching Total Sales by Country for year: {Year}", year);

        var sales = await _dbContext.Sales
            .Include(s => s.Country)
            .Include(s => s.InvoiceDate)
            .Where(s => s.InvoiceDate.InvoiceDate.Year == year)
            .ToListAsync();

        _logger.LogInformation("Fetched {Count} sales records", sales.Count);

        var totalSalesByCountry = sales
            .GroupBy(s => s.Country.CountryName)
            .Select(group => new TotalSalesByCountryViewModel
            {
                Country = group.Key,
                TotalSales = group.Sum(s => s.TotalPrice)
            })
            .ToList();

        _logger.LogInformation("Grouped into {GroupCount} countries", totalSalesByCountry.Count);

        return View("ViewForQuery2", totalSalesByCountry);
    }
    catch (Exception ex)
    {
        _logger.LogError("An error occurred in TotalSalesByCountry: {Error}", ex);
        return View("Error");
    }
}


        [HttpPost]
        public async Task<IActionResult> SalesByProduct(string productStockCode)
        {
            try
            {
                 // This should ideally be productStockCode
                _logger.LogInformation("Fetching Sales by Product for code: {productStockCode}", productStockCode);

                var salesByProduct = await _dbContext.Sales
                    .Include(s => s.Product)
                    .Include(s => s.InvoiceDate)
                    .Where(s => s.StockCode == productStockCode)
                    .Select(s => new SalesByProductViewModel
            		{
                		InvoiceNo = s.InvoiceNo,
                		Quantity = s.Quantity,
                		UnitPrice = s.UnitPrice,
                		TotalPrice = s.TotalPrice,
                		InvoiceDate = s.InvoiceDate.InvoiceDate
            		})
            		.ToListAsync();

                _logger.LogInformation("Retrieved {Count} product sales records", salesByProduct.Count);

                return View("ViewForQuery1", salesByProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in SalesByProduct: {Error}", ex);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SalesData()
        {
            try
            {
                _logger.LogInformation("Fetching Sales Data");

                var data = await _dbContext.Sales
                    .Include(s => s.Customer)
                    .Include(s => s.Country)
                    .Include(s => s.InvoiceDate)
                    .OrderBy(s => s.InvoiceNo)
                    .ThenByDescending(s => s.InvoiceDate)
                    .Take(10)
                    .ToListAsync();

                _logger.LogInformation("Fetched {Count} sales data records", data.Count);

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
