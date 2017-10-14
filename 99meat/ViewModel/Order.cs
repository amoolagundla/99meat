using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _99meat.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string ProdcutName { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryTime { get; set; }
        public string AddressId { get; set; }
        public decimal OrderTotal { get; set; }
        public string UserId { get; set; }
        public string OrderStatus { get; set; }
        public string thumb { get; set; }
    }

    public class UserOrderViewModel
    {
        private string _payment = Enum.Parse(typeof(Models.PaymentEnum), Models.PaymentEnum.NotPaid.ToString()).ToString();
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderAddress { get; set; }
        public string OrderStatus { get; set; }
        public int OrderId { get; set; }

        public string Payment
        {
            get
            {
                return _payment;
            }

            set
            {
                Payment = Enum.Parse(typeof(Models.PaymentEnum), PaymentMethod).ToString();
            }
        }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public Duration duration_in_traffic { get; set; }
        public string status { get; set; }
    }

    public class Row
    {
        public List<Element> elements { get; set; }
    }

    public class DistanceMatrix
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
        public string status { get; set; }
    }


    public class UserOrderDetailViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductAmount { get; set; }
        public string AddressId { get; set; }
        public string pic { get; set; }
        public int SpicyLevel { get; set; }
        public string Instructions { get; set; }
    }

    public class OrderView
    {
        [Required]
        public string DeliveryTime { get; set; }
        [Required]
        public string DeliveryDate { get; set; }
        public List<Cart> Cart { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required]
        public int PaymentMethod { get; set; }
        [Required]
        public int Amount { get; set; }
    }

    public class ProductView
    {
        [Required]
        public int Id { get; set; }
        public string ProdcutName { get; set; }
        public string ProductDesc { get; set; }
        public bool IsProductActive { get; set; }
        public int Price { get; set; }
        public string thumb { get; set; }
        public int Offer { get; set; }
        public int SpicyLevel { get; set; }
        public string Instructions { get; set; }
    }

    public class Cart
    {
        public string Id { get; set; }       
        [Required]
        public int Quantity { get; set; }
        [Required]
        public ProductView Product { get; set; }       
    }
}