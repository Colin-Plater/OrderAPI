using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderAPI.Models
{
    public class Order
    {
        public int Id;
        public string CustomerName;
        public DateTime OrderDate;
        public StatusEnum Status;
        public decimal TotalAmount;
        public List<OrderItem> Items;
    }

    public enum StatusEnum
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}