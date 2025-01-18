using SalesDatePrediction.Interfaces.RepositoryPattern;
using SalesDatePrediction.Models;

namespace SalesDatePrediction.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerOrderPrediction>> GetCustomerOrderPredictionsAsync();
        Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersAsync(int customerId);

        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync();

        Task<IEnumerable<ShipperDto>> GetShippersAsync();

        Task<IEnumerable<ProductDto>> GetProductsAsync();

        Task<int> CreateOrderAsync(OrderDto orderDto);

        Task<IEnumerable<CustomerOrderDto>> GetCustomerNameOrdersAsync(string customerName);

    }

  
}