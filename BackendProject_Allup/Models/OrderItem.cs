﻿namespace BackendProject_Allup.Models
{
    public class OrderItem:BaseEntity
    {
        public double Total { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
