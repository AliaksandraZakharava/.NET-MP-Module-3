using System.Collections.Generic;
using Task.Data;

namespace NETMP.Module3.LINQ
{
    public class CustomerRegistrationStatistics
    {
        public Customer Customer { get; set; }

        public int? RegistrationYear { get; set; }

        public int? RegistrationMonth { get; set; }
    }

    public class CategoryProductsStockData
    {
        public string Category { get; set; }

        public IEnumerable<Product> IsInStock { get; set; }

        public IEnumerable<Product> IsNotInStock { get; set; }
    }

    public class CityStatistics
    {
        public string City { get; set; }

        public decimal Profitability { get; set; }

        public double Intensity { get; set; }
    }

    public class CustomersPerPeriodStatistics<T>
    {
        public string Customer { get; set; }

        public IEnumerable<T> PerPeriodStatistics { get; set; }
    }

    public class PerPeriodStatistics
    {
        public int Period { get; set; }

        public decimal PeriodOrdersTotal { get; set; }
    }

    public class PerMonthAndYearStatistics
    {
        public int Year { get; set; }

        public IEnumerable<PerPeriodStatistics> PerMonthStatistics { get; set; }
    }
}
