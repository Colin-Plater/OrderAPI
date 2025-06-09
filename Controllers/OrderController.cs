using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrderAPI.Models;

namespace OrderAPI.Controllers
{
    public class OrderController : ApiController
    {
        public string CreateOrder(string CustomerName, decimal TotalAmount, List<OrderItem> Items)
        {
            Order order = new Order();
            order.Id = 0;
            order.CustomerName = CustomerName;
            order.OrderDate = DateTime.Now;
            order.Status = StatusEnum.Pending;
            order.TotalAmount = CaiculateTotalAmount(Items);
            order.Items = Items;

            string result = ValidateOrder(order, false);

            if (string.IsNullOrEmpty(result))
            {
                SaveOrderToDatabase(order);
            }

            return result;
        }

        public string UpdateOrder(int Id, string CustomerName, DateTime OrderDate, StatusEnum Status, decimal TotalAmount, List<OrderItem> Items)
        {
            Order order = GetOrderFromDatabase(Id);

            order.CustomerName = CustomerName;
            order.OrderDate = OrderDate;
            order.Status = Status;
            order.TotalAmount = CaiculateTotalAmount(Items);
            order.Items = Items;

            string result = ValidateOrder(order, true);

            if (string.IsNullOrEmpty(result))
            {
                SaveOrderToDatabase(order);
            }

            return result;
        }

        private Order GetOrderFromDatabase(int Id)
        {
            // Return an empty order as I don't have SQL Server 

            Order order = new Order();

            if (Id > 0)
                order.Id = Id;

            return order;
        }

        private void SaveOrderToDatabase(Order order)
        {
            // Do nothing as I don't have SQL Server
        }

        private string ValidateOrder(Order Order, bool Updating)
        {
            List<String> Errors = new List<string>();

            if (Updating && Order.Id == 0)
                Errors.Add("Order could not be found");

            if (string.IsNullOrEmpty(Order.CustomerName))
                Errors.Add("Customer Name must not be empty");
            else if (Order.CustomerName.Length > 100)
                Errors.Add("Customer Name must not exceed 100 characters");

            if (Order.Items.Count == 0)
                Errors.Add("An order must contain at least one Item");

            foreach (OrderItem item in Order.Items)
            {
                if (string.IsNullOrEmpty(item.ProductName))
                    Errors.Add("Product Name must not be empty");
                else if (item.ProductName.Length > 50)
                    Errors.Add("Product Name must not exceed 50 characters");

                if (item.Quantity <= 0)
                    Errors.Add("Quantity must be greater than zero");

                if (item.UnitPrice <= 0)
                    Errors.Add("Unit Price must be greater than zero");
            }

            if (Updating)
            {
                if (Order.Status == StatusEnum.Delivered || Order.Status == StatusEnum.Cancelled)
                    Errors.Add("Cannot update an order whose Status is Pending or Cancelled");
            }

            if (Errors.Count == 0)
                return "";
            else
            {
                if (Updating)
                    return "Could not update Order for the following reason(s): " + String.Join(", ", Errors);
                else
                    return "Could not insert Order for the following reason(s): " + String.Join(", ", Errors);
            }

        }

        private decimal CaiculateTotalAmount(List<OrderItem> Items)
        {
            decimal total = 0;

            foreach (OrderItem item in Items)
            {
                total = total + (item.Quantity * item.UnitPrice);
            }

            return total;
        }
    }
}
