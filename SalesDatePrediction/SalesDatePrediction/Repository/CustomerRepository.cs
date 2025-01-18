using Microsoft.EntityFrameworkCore;
using SalesDatePrediction.Interfaces;
using SalesDatePrediction.Interfaces.RepositoryPattern;
using SalesDatePrediction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Data.SqlClient; // Asegúrate de tener esto para LINQ

namespace SalesDatePrediction.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SalesDatePredictionDbContext _context;

        public CustomerRepository(SalesDatePredictionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<IEnumerable<CustomerOrderPrediction>> GetCustomerOrderPredictionsAsync()
        {
            var sqlQuery = @"
    WITH OrderDifferences AS (
        SELECT 
            O.custid,
            DATEDIFF(day, LAG(O.orderdate) OVER (PARTITION BY O.custid ORDER BY O.orderdate), O.orderdate) AS days_between_orders,
            O.orderdate AS last_order_date
        FROM Sales.Orders O
        WHERE O.custid IS NOT NULL
    )
    SELECT 
        C.companyname AS CustomerName,
        MAX(OD.last_order_date) AS LastOrderDate,
        DATEADD(
            day, 
            ISNULL(AVG(OD.days_between_orders), 0),
            MAX(OD.last_order_date)
        ) AS NextPredictedOrder
    FROM OrderDifferences OD
    JOIN Sales.Customers C ON OD.custid = C.custid
    GROUP BY C.companyname;
    ";

            // Ejecuta la consulta y mapea el resultado
            var predictions = await _context.CustomerOrderPredictions
                                             .FromSqlRaw(sqlQuery)
                                             .ToListAsync();

            return predictions;
        }


        public async Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersAsync(int customerId)
        {
            var sqlQuery = @"
    SELECT 
        O.OrderID,
        O.RequiredDate,
        O.ShippedDate,
        O.ShipName,
        O.ShipAddress,
        O.ShipCity
    FROM Sales.Orders O
    WHERE O.custid = @customerId;";

            var orders = await _context.CustomerOrders
                                        .FromSqlRaw(sqlQuery, new SqlParameter("@customerId", customerId))
                                        .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            var sqlQuery = @"
    SELECT 
        E.Empid,
        CONCAT(E.FirstName, ' ', E.LastName) AS FullName
    FROM hr.Employees E";

            var employees = await _context.Set<EmployeeDto>()
                                          .FromSqlRaw(sqlQuery)
                                          .ToListAsync();

            return employees;
        }

        public async Task<IEnumerable<ShipperDto>> GetShippersAsync()
        {
            var sqlQuery = @"
    SELECT 
        S.Shipperid,
        S.CompanyName
    FROM sales.Shippers S";

            var shippers = await _context.Set<ShipperDto>()
                                          .FromSqlRaw(sqlQuery)
                                          .ToListAsync();

            return shippers;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var sqlQuery = @"
    SELECT 
        P.Productid,
        P.ProductName
    FROM Production.Products P";

            var products = await _context.Set<ProductDto>()
                                          .FromSqlRaw(sqlQuery)
                                          .ToListAsync();

            return products;
        }

        public async Task<int> CreateOrderAsync(OrderDto orderDto)
        {
            // Validate input
            if (orderDto == null || orderDto.OrderDetails == null || !orderDto.OrderDetails.Any())
            {
                throw new ArgumentException("The order or its details cannot be null or empty.");
            }

            // Validate that all product IDs exist in the Products table
            var productIds = orderDto.OrderDetails.Select(detail => detail.Productid).ToList();

            // Use the actual Product entity class here
            var existingProductIds = await _context.Set<Product>()  // Use Product entity instead of ProductDto
                  .Where(p => productIds.Contains(p.ProductId))  // Correct ProductId property
                  .Select(p => p.ProductId)  // Select the product IDs
                  .ToListAsync();

            if (existingProductIds.Count != productIds.Count)
            {
                throw new ArgumentException("One or more product IDs are invalid.");
            }

            try
            {
                // Map DTO to entity
                var orderEntity = new OrderEntity
                {
                    EmpId = orderDto.Empid,
                    ShipperId = orderDto.Shipperid,
                    ShipName = orderDto.Shipname,
                    ShipAddress = orderDto.Shipaddress,
                    ShipCity = orderDto.Shipcity,
                    ShipRegion =  string.Empty,  // Correct ShipRegion property
                    OrderDate = orderDto.Orderdate,
                    RequiredDate = orderDto.Requireddate,
                    ShippedDate = orderDto.Shippeddate,
                    Freight = orderDto.Freight,
                    ShipCountry = orderDto.Shipcountry,
                    OrderDetails = orderDto.OrderDetails.Select(detail => new OrderDetail
                    {
                        ProductId = detail.Productid,
                        UnitPrice = detail.Unitprice,
                        Qty = (short)detail.Qty,  // Explicit cast to short
                        Discount = detail.Discount
                    }).ToList()
                };

                // Save to database
                _context.Orders.Add(orderEntity);
                await _context.SaveChangesAsync();

                // Return the created order's ID
                return orderEntity.OrderId;
            }
            catch (DbUpdateException dbEx)
            {
                // Log the error and provide specific feedback
                Console.WriteLine($"Database error occurred: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
                }
                throw new InvalidOperationException("An error occurred while saving the order to the database.", dbEx);
            }
            catch (ArgumentException argEx)
            {
                // Handle argument validation errors
                Console.WriteLine($"Argument error: {argEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Catch any other exceptions
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw new InvalidOperationException("An unexpected error occurred while processing the order.", ex);
            }
        }



        public async Task<IEnumerable<CustomerOrderDto>> GetCustomerNameOrdersAsync(string customerName)
        {
            var sqlQuery = @"
  SELECT 
    O.OrderID,
    O.RequiredDate,
    O.ShippedDate,
    O.ShipName,
    O.ShipAddress,
    O.ShipCity,
    c.custid
FROM Sales.Orders O
inner join Sales.[Customers] C on c.custid = o.custid
   WHERE c.companyname= @customerName;";

            var orders = await _context.CustomerOrders
                                        .FromSqlRaw(sqlQuery, new SqlParameter("@customerName", customerName))
                                        .ToListAsync();

            return orders;
        }







    }
}