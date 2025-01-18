using System.ComponentModel.DataAnnotations;

namespace SalesDatePrediction.Models
{
    public class Customer
    {
        [Key]
        public int custid { get; set; }  // Identificador del cliente

        public string companyname { get; set; }  // Nombre de la empresa
        public string contactname { get; set; }   // Nombre del contacto
        public string contacttitle { get; set; }  // Título del contacto
        public string address { get; set; }        // Dirección
        public string city { get; set; }           // Ciudad
        public string region { get; set; }         // Región
        public string postalcode { get; set; }     // Código postal
        public string country { get; set; }        // País
        public string phone { get; set; }          // Teléfono
        public string fax { get; set; }            // Fax
    }
}