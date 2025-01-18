namespace SalesDatePrediction.Models
{
    public class CustomerOrderDto
    {
        public int OrderId { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; } // Puede ser null si no se ha enviado
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
    }

}
