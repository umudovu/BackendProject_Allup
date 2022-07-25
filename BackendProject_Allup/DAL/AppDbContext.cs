using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }   

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Bio> Bios { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<TagProduct> TagProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            #region Brands

            #region Sliders

            builder.Entity<Slider>().HasData(
                new Slider
                {
                    Id = 1,
                    Title = "2079 Virtual Reality",
                    DiscountSec = "Save $666 when you buy",
                    Desc = "Explore and immerse in exciting 360 content with Fulldive’s all-in-one virtual reality platform",
                    ImageUrl = "images/slider-1.jpg",
                });

            builder.Entity<Slider>().HasData(
                new Slider
                {
                    Id = 2,
                    Title = "VR Cakera",
                    DiscountSec = "Endirimde",
                    Desc = "Qacirmayin",
                    ImageUrl = "images/slider-2.jpg",
                });

            #endregion




            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 1,
                    Name = "Adidas",
                    ImageUrl= "images/brand/brand-1.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 2,
                    Name = "Samsung",
                    ImageUrl= "images/brand/brand-2.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 3,
                    Name = "Asus",
                    ImageUrl= "images/brand/brand-3.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 4,
                    Name = "Philips",
                    ImageUrl= "images/brand/brand-4.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 5,
                    Name = "Sony",
                    ImageUrl= "images/brand/brand-5.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 6,
                    Name = "Xiaomi",
                    ImageUrl= "images/brand/brand-6.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 7,
                    Name = "Apple",
                    ImageUrl= "images/brand/brand-3.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 8,
                    Name = "Gucci",
                    ImageUrl= "images/brand/brand-4.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 9,
                    Name = "Pegasus",
                    ImageUrl= "images/brand/brand-1.jpg",
                    CreatedAt = DateTime.Now,
                });

            builder.Entity<Brand>().HasData(
               new Brand
               {
                   Id = 10,
                   Name = "Logitech",
                    ImageUrl= "images/brand/brand-5.jpg",
                   CreatedAt = DateTime.Now,
               });

            #endregion
            #region Categories

            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 2,
                   ImageUrl = "images/category-1.jpg",
                   ParentId = 1,
                   IsDeleted = false,
                   Name = "Laptop",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 1,
                   ImageUrl = "images/category-2.jpg",
                   IsDeleted = false,
                   Name = "Computer",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 3,
                   ImageUrl = "images/category-3.jpg",
                   IsDeleted = false,
                   Name = "Smartphone",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 4,
                   ImageUrl = "images/category-4.jpg",
                   IsDeleted = false,
                   Name = "Game Consoles",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 5,
                   ImageUrl = "images/category-5.jpg",
                   IsDeleted = false,
                   Name = "Bottoms",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 6,
                   ImageUrl = "images/category-6.jpg",
                   IsDeleted = false,
                   Name = "Tops & Sets",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 7,
                   ImageUrl = "images/category-7.jpg",
                   IsDeleted = false,
                   Name = "Audio & Video",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 8,
                   ImageUrl = "images/category-10.jpg",
                   ParentId = 7,
                   IsDeleted = false,
                   Name = "Camera",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 9,
                   ImageUrl = "images/category-9.jpg",
                   IsDeleted = false,
                   Name = "Household",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 10,
                   ImageUrl = "images/category-8.jpg",
                   IsDeleted = false,
                   Name = "Accessories",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 11,
                   ImageUrl = "images/category-11.jpg",
                   ParentId = 7,
                   IsDeleted = false,
                   Name = "Michrophone",
                   CreatedAt = DateTime.Now
               });
            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 12,
                   ImageUrl = "images/category-12.jpg",
                   ParentId = 4,
                   IsDeleted = false,
                   Name = "Video Games",
                   CreatedAt = DateTime.Now
               });

            builder.Entity<Category>().HasData(
               new Category
               {
                   Id = 13,
                   ImageUrl = "images/category-1.jpg",
                   IsDeleted = false,
                   Name = "Technologics",
                   CreatedAt = DateTime.Now
               });


            #endregion

            #region Product

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    CreatedAt = DateTime.Now,
                    NewArrival = true,
                    InStock = true,
                    BestSeller = false,
                    IsFeatured = false,
                    Name = "Joystic Logitech g66",
                    Price = 900,
                    StockCount = 33,
                    CategoryId = 4,
                    BrandId = 10,
                    DiscountPrice = 30,
                    TaxPercent = 0,
                });

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 2,
                    CreatedAt = DateTime.Now,
                    NewArrival = false,
                    BestSeller = true,
                    IsFeatured = false,
                    InStock = true,
                    Name = "Qulaqliqs",
                    Price = 250,
                    StockCount = 30,
                    CategoryId = 10,
                    BrandId = 5,
                    DiscountPrice = 30,
                    TaxPercent = 0,
                });

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 3,
                    CreatedAt = DateTime.Now,
                    NewArrival = false,
                    BestSeller = false,
                    IsFeatured = true,
                    InStock = true,
                    Name = "Flashcard",
                    Price = 75,
                    StockCount = 25,
                    CategoryId = 10,
                    BrandId = 4,
                    DiscountPrice = 0,
                    TaxPercent = 0,
                });


            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 4,
                    CreatedAt = DateTime.Now,
                    NewArrival = true,
                    BestSeller = true,
                    IsFeatured = false,
                    InStock = true,
                    Name = "Printer (Samsung Yta-55)",
                    Price = 300,
                    StockCount = 25,
                    CategoryId = 7,
                    BrandId = 2,
                    DiscountPrice = 0,
                    TaxPercent = 0,
                });

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 5,
                    CreatedAt = DateTime.Now,
                    NewArrival = true,
                    BestSeller = false,
                    IsFeatured = true,
                    InStock = true,
                    Name = "Drone",
                    Price = 3000,
                    StockCount = 9,
                    CategoryId = 13,
                    BrandId = 2,
                    DiscountPrice = 0,
                    TaxPercent = 0,
                });

            builder.Entity<Product>().HasData(
               new Product
               {
                   Id = 6,
                   CreatedAt = DateTime.Now,
                   NewArrival = false,
                   BestSeller = true,
                   IsFeatured = true,
                   InStock = true,
                   Name = "Sunglasses",
                   Price = 99,
                   StockCount = 800,
                   CategoryId = 10,
                   BrandId = 1,
                   DiscountPrice = 0,
                   TaxPercent = 0,
               });
            builder.Entity<Product>().HasData(
               new Product
               {
                   Id = 7,
                   CreatedAt = DateTime.Now,
                   NewArrival = false,
                   BestSeller = false,
                   IsFeatured = true,
                   InStock = true,
                   Name = "PhotoAparatte",
                   Price = 199,
                   StockCount = 75,
                   CategoryId = 13,
                   BrandId = 3,
                   DiscountPrice = 0,
                   TaxPercent = 0,
               });

            #endregion

            #region ProductImages

            builder.Entity<ProductImage>().HasData(
               new ProductImage
               {
                   Id = 1,
                   ImageUrl = "images/product/product-3.jpg",
                   IsMain = true,
                   ProductId = 1,
               });

            builder.Entity<ProductImage>().HasData(
               new ProductImage
               {
                   Id = 2,
                   ImageUrl = "images/product/product-9.jpg",
                   IsMain = false,
                   ProductId = 1,
               });

            builder.Entity<ProductImage>().HasData(
               new ProductImage
               {
                   Id = 3,
                   ImageUrl = "images/product/product-1.jpg",
                   IsMain = true,
                   ProductId = 4,
               });

            builder.Entity<ProductImage>().HasData(
               new ProductImage
               {
                   Id = 4,
                   ImageUrl = "images/product/product-2.jpg",
                   IsMain = false,
                   ProductId = 4,
               });

            builder.Entity<ProductImage>().HasData(
               new ProductImage
               {
                   Id = 5,
                   ImageUrl = "images/product/product-5.jpg",
                   IsMain = true,
                   ProductId = 2,
               });

            builder.Entity<ProductImage>().HasData(
               new ProductImage
               {
                   Id = 6,
                   ImageUrl = "images/product/product-6.jpg",
                   IsMain = false,
                   ProductId = 2,
               });

            builder.Entity<ProductImage>().HasData(
               new ProductImage
               {
                   Id = 7,
                   ImageUrl = "images/product/product-14.jpg",
                   IsMain = true,
                   ProductId = 5,
               });
            builder.Entity<ProductImage>().HasData(
              new ProductImage
              {
                  Id = 8,
                  ImageUrl = "images/product/product-15.jpg",
                  IsMain = false,
                  ProductId = 5,
              });
            builder.Entity<ProductImage>().HasData(
              new ProductImage
              {
                  Id = 9,
                  ImageUrl = "images/product/product-8.jpg",
                  IsMain = true,
                  ProductId = 3,
              });
            builder.Entity<ProductImage>().HasData(
              new ProductImage
              {
                  Id = 10,
                  ImageUrl = "images/product/product-7.jpg",
                  IsMain = false,
                  ProductId = 3,
              });
            builder.Entity<ProductImage>().HasData(
              new ProductImage
              {
                  Id = 11,
                  ImageUrl = "images/product/product-13.jpg",
                  IsMain = true,
                  ProductId = 6,
              });
            builder.Entity<ProductImage>().HasData(
              new ProductImage
              {
                  Id = 12,
                  ImageUrl = "images/product/product-12.jpg",
                  IsMain = false,
                  ProductId = 6,
              });

            builder.Entity<ProductImage>().HasData(
             new ProductImage
             {
                 Id = 13,
                 ImageUrl = "images/product/product-10.jpg",
                 IsMain = true,
                 ProductId = 7,
             });
            builder.Entity<ProductImage>().HasData(
             new ProductImage
             {
                 Id = 14,
                 ImageUrl = "images/product/product-11.jpg",
                 IsMain = false,
                 ProductId = 7,
             });

            #endregion
        }


    }
   }
