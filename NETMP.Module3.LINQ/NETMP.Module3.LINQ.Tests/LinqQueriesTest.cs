using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace NETMP.Module3.LINQ.Tests
{
    public class LinqQueriesTest
    {
        private const string TestResultsOutputPath = @"..\..\..\TestResults.txt";

        private readonly LinqQueries _queries;
        private readonly StringBuilder _stringBuilder;

        public LinqQueriesTest()
        {
            _queries = new LinqQueries();
            _stringBuilder = new StringBuilder();
        }

        [Fact]
        public void LINQ1_CustomersWithOrdersSumMoreThan_Test()
        {
            var limit = 10000;

            var actualResult = _queries.LINQ1_CustomersWithOrdersSumMoreThan(limit);

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var customer in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{customer.CompanyName}- {customer.Orders.Sum(order => order.Total)}");
            }

            WriteResultsToFile($"LINQ1_CustomersWithOrdersSumMoreThan_{limit}", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithGrouping_Test()
        {
            var actualResult = _queries.LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithGrouping();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var group in actualResult)
            {
                _stringBuilder.AppendLine($"{group.Key.CompanyName} - {group.Key.Country} - {group.Key.City}");
                group.ToList().ForEach(suppler => _stringBuilder.AppendLine($"- {suppler.SupplierName}- {suppler.Country} - {suppler.City}"));
            }

            WriteResultsToFile("LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithGrouping", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithoutGrouping_Test()
        {
            var actualResult = _queries.LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithoutGrouping();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var group in actualResult)
            {
                _stringBuilder.AppendLine($"{group.Key.CompanyName}");
                group.Value.ForEach(suppler => _stringBuilder.AppendLine($"- {suppler.SupplierName}- {suppler.Country} - {suppler.City}"));
            }

            WriteResultsToFile("LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithoutGrouping", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ3_CustomersHavingOrdersMoreExpensiveThan_Test()
        {
            var limit = 10000;

            var actualResult = _queries.LINQ3_CustomersHavingOrdersMoreExpensiveThan(limit);

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var customer in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{customer.CompanyName}- {customer.Orders.Where(order => order.Total > limit).Count()}");
            }

            WriteResultsToFile($"LINQ3_CustomersHavingOrdersMoreExpensiveThan_{limit}", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ4_CustomerRegistrationStatistics_Test()
        {
            var actualResult = _queries.LINQ4_CustomerRegistrationStatistics();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var statistics in actualResult.Take(10))
            {
                var month = statistics.RegistrationMonth.HasValue ? statistics.RegistrationMonth.ToString() : "no set";
                var year = statistics.RegistrationYear.HasValue ? statistics.RegistrationYear.ToString() : "no set";

                _stringBuilder.AppendLine($"{statistics.Customer.CompanyName} - Registration : {month}/{year}");
            }

            WriteResultsToFile("LINQ4_CustomerRegistrationStatistics", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ5_CustomerRegistrationStatistics_Sorted_Test()
        {
            var actualResult = _queries.LINQ5_CustomerRegistrationStatistics_Sorted();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var statistics in actualResult.Take(10))
            {
                var month = statistics.RegistrationMonth.HasValue ? statistics.RegistrationMonth.ToString() : "not set";
                var year = statistics.RegistrationYear.HasValue ? statistics.RegistrationYear.ToString() : "not set";

                _stringBuilder.AppendLine($"{statistics.Customer.CompanyName} - Registration : {month}/{year}");
            }

            WriteResultsToFile("LINQ5_CustomerRegistrationStatistics_Sorted", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ6_CustomerWithInValidContactData_Test()
        {
            var actualResult = _queries.LINQ6_CustomerWithInValidContactData();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var customer in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{customer.CompanyName} - Post code : {customer.PostalCode}, Region: {customer.Region}, Phone: {customer.Phone}");
            }

            WriteResultsToFile("LINQ6_CustomerWithInValidContactData", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ7_GroupProductsByCategoryInsideByInStockInsideTheLastByPrice_Test()
        {
            var actualResult = _queries.LINQ7_GroupProductsByCategoryInsideByInStockInsideTheLastByPrice();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var category in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{category.Category}");
                _stringBuilder.AppendLine("\tIs in stock (first 10, sorted by price):");
                category.IsInStock.Take(10).ToList().ForEach(product => _stringBuilder.AppendLine($"\t\t-{product.ProductName} - {product.UnitPrice}"));
                _stringBuilder.AppendLine("\tIs not in stock (first 10):");
                category.IsInStock.ToList().ForEach(product => _stringBuilder.AppendLine($"\t\t-{product.ProductName}"));
            }

            WriteResultsToFile("LINQ7_GroupProductsByCategoryInsideByInStockInsideTheLastByPrice", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ8_GroupProductsByPriceCategories_Test()
        {
            var lowCostLimit = 10;
            var mediumCostLimit = 40;

            var actualResult = _queries.LINQ8_GroupProductsByPriceCategories(lowCostLimit, mediumCostLimit);

            Assert.NotNull(actualResult);

            foreach (var group in actualResult)
            {
                _stringBuilder.AppendLine($"{group.Key} (first 10):");
                group.Take(10).ToList().ForEach(product => _stringBuilder.AppendLine($"- {product.ProductName} - {product.UnitPrice}"));
            }
            WriteResultsToFile("LINQ8_GroupProductsByPriceCategories_Test", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ9_GetCitiesProfitabilityAndIntensityStatistics_Test()
        {
            var actualResult = _queries.LINQ9_GetCitiesProfitabilityAndIntensityStatistics();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var city in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{city.City} - profitablility {string.Format("{0:0.00}", city.Profitability)}, intensity {string.Format("{0:0.00}", city.Intensity)}");
            }

            WriteResultsToFile("LINQ9_GetCitiesProfitabilityAndIntensityStatistics", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ10_GetCustomersPerMonthStatistics_Test()
        {
            var actualResult = _queries.LINQ10_GetCustomersPerMonthStatistics();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var customer in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{customer.Customer}");
                customer.PerPeriodStatistics.ToList().ForEach(stat => _stringBuilder.AppendLine($"- month {stat.Period}: {stat.PeriodOrdersTotal}"));
            }

            WriteResultsToFile("LINQ10_GetCustomersPerMonthStatistics", _stringBuilder.ToString());
        }

        [Fact]
        public void LINQ10_GetCustomersPerYearStatistics_Test()
        {
            var actualResult = _queries.LINQ10_GetCustomersPerYearStatistics();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var customer in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{customer.Customer}");
                customer.PerPeriodStatistics.ToList().ForEach(stat => _stringBuilder.AppendLine($"- year {stat.Period}: {stat.PeriodOrdersTotal}"));
            }

            WriteResultsToFile("LINQ10_GetCustomersPerYearStatistics", _stringBuilder.ToString());
        }


        [Fact]
        public void LINQ10_GetCustomersPerMonthAndYearsStatistics_Test()
        {
            var actualResult = _queries.LINQ10_GetCustomersPerMonthAndYearsStatistics();

            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

            foreach (var customer in actualResult.Take(10))
            {
                _stringBuilder.AppendLine($"{customer.Customer}");

                foreach (var stat in customer.PerPeriodStatistics)
                {
                    _stringBuilder.AppendLine($"- year {stat.Year}");
                    stat.PerMonthStatistics.ToList().ForEach(month => _stringBuilder.AppendLine($"\t- month {month.Period}: {month.PeriodOrdersTotal}"));
                }
            }

            WriteResultsToFile("LINQ10_GetCustomersPerMonthAndYearsStatistics", _stringBuilder.ToString());
        }

        private void WriteResultsToFile(string methodName, string dataToWrite)
        {
            var data = File.ReadAllText(TestResultsOutputPath);

            var outputData = $"{data}{Environment.NewLine}{methodName} results (first 10): {Environment.NewLine}{Environment.NewLine}{dataToWrite}";

            File.WriteAllText(TestResultsOutputPath, outputData);
        }
    }
}
