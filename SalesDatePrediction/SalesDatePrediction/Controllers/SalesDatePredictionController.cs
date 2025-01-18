using Microsoft.AspNetCore.Mvc;
using System.Linq;  // Para usar el método Any() 
using System.Threading.Tasks;
using SalesDatePrediction.Interfaces;
using SalesDatePrediction.Repositories;
using SalesDatePrediction.Models; // Para importar la interfaz ICustomerRepository

namespace SalesDatePrediction.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        // Inyección de dependencias en el constructor
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Acción para predecir la próxima orden
        [HttpGet("NextOrderPrediction")]
        public async Task<IActionResult> GetNextOrderPrediction()
        {
            try
            {
                
                var predictions = await _customerRepository.GetCustomerOrderPredictionsAsync();

                return Ok(predictions);
            }
            catch (Exception ex)
            {
            
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet("GetClientNameOrders/{customerId}")]
        public async Task<IActionResult> GetClientOrders(int customerId)
        {
            var orders = await _customerRepository.GetCustomerOrdersAsync(customerId);

            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "No orders found for the given customer." });
            }

            return Ok(orders);
        }

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _customerRepository.GetEmployeesAsync();
            return Ok(employees);
        }
        [HttpGet("GetShippers")]
        public async Task<IActionResult> GetShippers()
        {
            var shippers = await _customerRepository.GetShippersAsync();
            return Ok(shippers);
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _customerRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Invalid order data.");
            }

            try
            {
                var orderId = await _customerRepository.CreateOrderAsync(orderDto);
                return Ok(new { OrderId = orderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetClientOrders/{customerName}")]
        public async Task<IActionResult> GetClientOrders(string customerName)
        {
            var orders = await _customerRepository.GetCustomerNameOrdersAsync(customerName);

            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "No orders found for the given customer." });
            }

            return Ok(orders);
        }

    }
}
