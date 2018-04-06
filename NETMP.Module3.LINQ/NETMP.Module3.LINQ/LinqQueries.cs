using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Task.Data;

namespace NETMP.Module3.LINQ
{
    public class LinqQueries
    {
        private DataSource _dataSource;

        public LinqQueries()
        {
            _dataSource = new DataSource();
        }

        public IEnumerable<Customer> LINQ1_CustomersWithOrdersSumMoreThan(decimal value)
        {
            return _dataSource.Customers.Where(customer => customer.Orders.Sum(order => order.Total) > value);
        }

        public IEnumerable<IGrouping<Customer, Supplier>> LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithGrouping()
        {
            return _dataSource.Customers.Join(_dataSource.Suppliers,
                                              customer => new {customer.Country, customer.City},
                                              supplier => new {supplier.Country, supplier.City},
                                              (c, s) => new
                                              {
                                                  Customer = c,
                                                  Supplier = s
                                              })
                                        .GroupBy(customer => customer.Customer, customer => customer.Supplier);
        }

        public IDictionary<Customer, List<Supplier>> LINQ2_SuppliersAreInTheSameCountryAndCityAsACustomer_WithoutGrouping()
        {
            var result = new Dictionary<Customer, List<Supplier>>();

            var customersJoinSuppliers = _dataSource.Customers.Join(_dataSource.Suppliers,
                                                                    customer => new {customer.Country, customer.City},
                                                                    supplier => new {supplier.Country, supplier.City},
                                                                    (c, s) => new
                                                                    {
                                                                        Customer = c,
                                                                        Supplier = s
                                                                    });

            foreach (var pair in customersJoinSuppliers)
            {
                if (!result.ContainsKey(pair.Customer))
                {
                    result.Add(pair.Customer, new List<Supplier>());
                }

                result[pair.Customer].Add(pair.Supplier);
            }

            return result;
        }

        public IEnumerable<Customer> LINQ3_CustomersHavingOrdersMoreExpensiveThan(decimal value)
        {
            return _dataSource.Customers.Where(customer => customer.Orders.Any(order => order.Total > value));
        }

        public IEnumerable<CustomerRegistrationStatistics> LINQ4_CustomerRegistrationStatistics()
        {
            return _dataSource.Customers.Select(customer => new CustomerRegistrationStatistics
                                                            {
                                                                Customer = customer,
                                                                RegistrationYear = customer.Orders.Any() ? customer.Orders.Min(order => order.OrderDate).Year : (int?) null,
                                                                RegistrationMonth = customer.Orders.Any()? customer.Orders.Min(order => order.OrderDate).Month : (int?) null
            });
        }

        public IEnumerable<CustomerRegistrationStatistics> LINQ5_CustomerRegistrationStatistics_Sorted()
        {
            return LINQ4_CustomerRegistrationStatistics().OrderBy(customer => customer.RegistrationYear)
                                                         .ThenBy(customer => customer.RegistrationMonth)
                                                         .ThenByDescending(customer => customer.Customer.Orders.Sum(order => order.Total))
                                                         .ThenBy(customer => customer.Customer.CompanyName).AsEnumerable();
        }

        public IEnumerable<Customer> LINQ6_CustomerWithInValidContactData()
        {
            return _dataSource.Customers.Where(customer => !customer.PostalCode.All(Char.IsDigit) ||
                                                           !customer.Phone.ToCharArray().Contains('(') ||
                                                           string.IsNullOrEmpty(customer.Region));
        }

        public IEnumerable<CategoryProductsStockData> LINQ7_GroupProductsByCategoryInsideByInStockInsideTheLastByPrice()
        {
            return _dataSource.Products.GroupBy(product => product.Category)
                                       .Select(group => new CategoryProductsStockData
                                       {
                                            Category = group.Key,
                                            IsNotInStock = group.Where(product => product.UnitsInStock == 0),
                                            IsInStock = group.Where(product => product.UnitsInStock > 0).OrderBy(product => product.UnitPrice)
                                       });
        }

        public IEnumerable<IGrouping<string, Product>> LINQ8_GroupProductsByPriceCategories(decimal lowCostLimit, decimal mediumCostLimit)
        {
            return _dataSource.Products.GroupBy(product => product.UnitPrice <= lowCostLimit ? "Low cost" :
                                                           product.UnitPrice <= mediumCostLimit ? "Medium cost" : "High cost");
        }

        public IEnumerable<CityStatistics> LINQ9_GetCitiesProfitabilityAndIntensityStatistics()
        {
            return _dataSource.Customers.GroupBy(customer => customer.City)
                                        .Select(group => new CityStatistics
                                                        {
                                                           City = group.Key,
                                                           Profitability = group.Average(customer => customer.Orders.Sum(order => order.Total)),
                                                           Intensity = group.Average(customer => customer.Orders.Length)
                                                        });
        }

        public IEnumerable<CustomersPerPeriodStatistics<PerPeriodStatistics>> LINQ10_GetCustomersPerMonthStatistics()
        {
            return _dataSource.Customers.Select(customer => new CustomersPerPeriodStatistics<PerPeriodStatistics>
            {
                                                    Customer = customer.CompanyName,
                                                    PerPeriodStatistics = customer.Orders.GroupBy(order => order.OrderDate.Month)
                                                                                        .OrderBy(group => group.Key)
                                                                                        .Select(group => new PerPeriodStatistics
                                                                                                         {
                                                                                                             Period = group.Key,
                                                                                                             PeriodOrdersTotal = group.Sum(order => order.Total)
                                                                                                         })
                                                });
        }

        public IEnumerable<CustomersPerPeriodStatistics<PerPeriodStatistics>> LINQ10_GetCustomersPerYearStatistics()
        {
            return _dataSource.Customers.Select(customer => new CustomersPerPeriodStatistics<PerPeriodStatistics>
                                                            {
                                                                Customer = customer.CompanyName,
                                                                PerPeriodStatistics = customer.Orders.GroupBy(order => order.OrderDate.Year)
                                                                                                     .OrderBy(group => group.Key)
                                                                                                     .Select(group => new PerPeriodStatistics
                                                                                                                      {
                                                                                                                          Period = group.Key,
                                                                                                                          PeriodOrdersTotal = group.Sum(order => order.Total)
                                                                                                                      })
                                                            });
        }

        public IEnumerable<CustomersPerPeriodStatistics<PerMonthAndYearStatistics>> LINQ10_GetCustomersPerMonthAndYearsStatistics()
        {
            return _dataSource.Customers.Select(customer => new CustomersPerPeriodStatistics<PerMonthAndYearStatistics>
                                                            {
                                                                Customer = customer.CompanyName,
                                                                PerPeriodStatistics = customer.Orders.GroupBy(order => order.OrderDate.Year)
                                                                                                     .OrderBy(group => group.Key)
                                                                                                     .Select(group => new PerMonthAndYearStatistics
                                                                                                                      {
                                                                                                                            Year = group.Key,
                                                                                                                            PerMonthStatistics = group.GroupBy(order => order.OrderDate.Month)
                                                                                                                                                      .OrderBy(g => group.Key)
                                                                                                                                                      .Select(g => new PerPeriodStatistics
                                                                                                                                                                   {
                                                                                                                                                                       Period = g.Key,
                                                                                                                                                                       PeriodOrdersTotal = group.Sum(order => order.Total)
                                                                                                                                                                   })
                                                                                                                      })
                                                            });
        }
    }
}
