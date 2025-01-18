using System.ComponentModel.DataAnnotations;

namespace SalesDatePrediction.Models
{
    public class CustomerOrderPrediction
    {
        public string CustomerName { get; set; }
        public DateTime LastOrderDate { get; set; }
        public DateTime NextPredictedOrder { get; set; }
    }
}