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

        public string Email { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderAddress { get; set; }
        public string OrderStatus { get; set; }
        public int OrderId { get; set; }
    }
    public class UserOrderDetailViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductAmount { get; set; }
    }

    public class Order
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
    }

    public class Product
    {
        [Required]
        public int Id { get; set; }
        public string ProdcutName { get; set; }
        public string ProductDesc { get; set; }
        public bool IsProductActive { get; set; }
        public int Price { get; set; }
        public string thumb { get; set; }
        public int Offer { get; set; }
    }

    public class Cart
    {
        public string Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int UnitPrice { get; set; }
        public Product Product { get; set; }
        public int total { get; set; }
    }
}