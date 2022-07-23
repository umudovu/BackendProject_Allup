﻿namespace BackendProject_Allup.ViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public int CategoryId { get; set; }
        public double SubTotal { get; set; }
        public int BasketCount { get; set; }
    }
}
