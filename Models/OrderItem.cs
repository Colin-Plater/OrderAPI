using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderAPI.Models
{
    public class OrderItem
    {
        public int Id;
        public string ProductName;
        public int Quantity;
        public decimal UnitPrice;
        public int OrderId;
    }
}