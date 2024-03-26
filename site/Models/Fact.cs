using System.ComponentModel.DataAnnotations;
using System;

namespace Site.Models
{
    public class Sale
    {
        public Sale() { }

        public Sale(string invoiceNo, string stockCode, int quantity, decimal unitPrice, 
            int customerId, int countryId, int dateId, string productStockCode)
        {
            InvoiceNo = invoiceNo;
            StockCode = stockCode;
            Quantity = quantity;
            UnitPrice = unitPrice;
            CustomerID = customerId;
            CountryID = countryId;
            InvoiceDateID = dateId;
            ProductStockCode = productStockCode;
        }

        [Key]
        public int SalesID { get; set; }
        public string InvoiceNo { get; set; }
        public string StockCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get { return Quantity * UnitPrice; } }
        public int InvoiceDateID { get; set; }
        public int CustomerID { get; set; }
        public int CountryID { get; set; }
        public string ProductStockCode { get; set; }

        // Navigation properties
        public virtual Date InvoiceDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Country Country { get; set; }
        public virtual Product Product { get; set; }
    }
	


    public class Date
    {
        public Date() { }

        public Date(DateTime invoiceDate)
        {
            InvoiceDate = invoiceDate;
               Year = invoiceDate.Year;
        Month = invoiceDate.Month;
        Day = invoiceDate.Day;
        }

        
        [Key]
        public int DateID { get; set; } 
        public DateTime InvoiceDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }

    public class Product
    {
        public Product() { }

        public Product(string stockCode, string description)
        {
            StockCode = stockCode;
            Description = description;
        }

        [Key] // Define a primary key for the Product entity
        public string StockCode { get; set; }
        public string Description { get; set; }
    }

    public class Customer
    {
        public Customer() { }

        public Customer(int customerId)
        {
            CustomerID = customerId;
            // Initialize other properties as needed
        }

        public int CustomerID { get; set; }
        // Additional attributes...
    }

    public class Country
    {
        public Country() { }

        public Country(int countryId, string countryName)
        {
            CountryID = countryId;
            CountryName = countryName;
        }

        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
}


public class SalesByProductViewModel
{
    public string InvoiceNo { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime InvoiceDate { get; set; }
}

public class TotalSalesByCountryViewModel
{
    public string Country { get; set; }
    public decimal TotalSales { get; set; }
}

