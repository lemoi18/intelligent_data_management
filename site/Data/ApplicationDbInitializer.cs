using System;
using System.IO;
using System.Linq;
using Site.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Collections.Generic;

namespace Site.Data
{
    public class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db)
        {
            db.Database.EnsureCreated();

            var csvFilePath = "Data/data.csv";

            // Check if any of the required tables exist in the database
            if (!db.Customers.Any() || !db.Products.Any() || !db.Countries.Any() || !db.Dates.Any())
            {
                // If any of the required tables are empty, populate them from CSV file
                PopulateDataFromCsv2(db, csvFilePath);
            }

        }
        private static void PopulateDataFromCsv2(ApplicationDbContext db, string csvFilePath)
{
    var customerIds = new HashSet<int>();
    var productStockCodes = new HashSet<string>();
    var countryNames = new HashSet<string>();
    var dates = new HashSet<DateTime>();
    var sales = new List<Sale>();

    using (var reader = new StreamReader(csvFilePath))
    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null, MissingFieldFound = null
    }))
    {
        csv.Read();
        csv.ReadHeader(); 
        while (csv.Read())
        {
            try
            {
                var invoiceNo = csv.GetField<string>("InvoiceNo");
                var stockCode = csv.GetField<string>("StockCode");
                var description = csv.GetField<string>("Description");
                var quantity = csv.GetField<int>("Quantity");
                var unitPrice = csv.GetField<decimal>("UnitPrice");
                var invoiceDateStr = csv.GetField<string>("InvoiceDate");
                var customerId = csv.GetField<int>("CustomerID");
                var countryName = csv.GetField<string>("Country");

                if (!DateTime.TryParse(invoiceDateStr, out var invoiceDateValue))
                {
                    continue; // or log the error
                }

                dates.Add(invoiceDateValue);
                customerIds.Add(customerId);
                productStockCodes.Add(stockCode);
                countryNames.Add(countryName);

                // Constructing Sale object
                var sale = new Sale
                {
                    InvoiceNo = invoiceNo,
                    StockCode = stockCode,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    CustomerID = customerId,
                    CountryID = countryName.GetHashCode(), // Temporary, replace with actual ID retrieval logic
                    InvoiceDate = new Date(invoiceDateValue) // Replace with actual Date retrieval logic
                    // Assign other properties as necessary
                };
                sales.Add(sale);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine(ex.Message); // Modify with your logging approach
            }
        }
    }

    // Bulk add dates, customers, products, and countries
    AddDateRecords(db, dates);
    AddCustomerRecords(db, customerIds);
    AddProductRecords(db, productStockCodes);
    AddCountryRecords(db, countryNames);

    db.Sales.AddRange(sales);
    db.SaveChanges();
}

private static void AddDateRecords(ApplicationDbContext db, HashSet<DateTime> dates)
{
    var existingDates = db.Dates.Select(d => d.InvoiceDate).ToHashSet();
    var newDates = dates.Except(existingDates).Select(d => new Date(d));
    db.Dates.AddRange(newDates);
}

private static void AddCustomerRecords(ApplicationDbContext db, HashSet<int> customerIds)
{
    var existingCustomerIds = db.Customers.Select(c => c.CustomerID).ToHashSet();
    var newCustomers = customerIds.Except(existingCustomerIds).Select(id => new Customer(id));
    db.Customers.AddRange(newCustomers);
}

private static void AddProductRecords(ApplicationDbContext db, HashSet<string> productStockCodes)
{
    var existingStockCodes = db.Products.Select(p => p.StockCode).ToHashSet();
    var newProducts = productStockCodes.Except(existingStockCodes).Select(code => new Product(code, "Default Description")); // Modify as needed
    db.Products.AddRange(newProducts);
}

private static void AddCountryRecords(ApplicationDbContext db, HashSet<string> countryNames)
{
    var existingCountryNames = db.Countries.Select(c => c.CountryName).ToHashSet();
    var newCountries = countryNames.Except(existingCountryNames).Select(name => new Country(name.GetHashCode(), name)); // Modify as needed
    db.Countries.AddRange(newCountries);
}

    }
}