using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SelectPdf;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Extentions;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;




namespace BackendProject_Allup.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }
        public IActionResult Index()
        {
          
            return RedirectToAction("Checkout");
        }

        public IActionResult Checkout()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order order = new Order();

            var basket = _context.Baskets.Include(b => b.BasketItems).ThenInclude(b => b.Product).FirstOrDefault(b => b.UserId == userId);
            
            if(basket.BasketItems.Count==0) return RedirectToAction("Index","shop");

            ViewBag.BasketItems = basket.BasketItems.ToList();

            return View(order);
        }

        [HttpPost]
        public IActionResult Sale(Order order, string radio = "Cash")
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Error has been occured");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);

            var basket = _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(b => b.Product)
                .FirstOrDefault(b => b.UserId == userId);

            var basketItems = basket.BasketItems.ToList();
            double total = 0;

            foreach (var item in basketItems)
            {
                total += item.Product.Price * item.Count;
            }
            if (user.Balance <= total)
            {
                return RedirectToAction("index", "home");
            }
            user.Balance -= total;
            Random random = new Random();

            Order newOrder = new Order();

            newOrder.Address = order.Address;
            newOrder.City = order.City;
            newOrder.Country = order.Country;
            newOrder.CreatedAt = DateTime.Now;
            newOrder.Email = order.Email;
            newOrder.FirstName = order.FirstName;
            newOrder.Surname = order.Surname;
            newOrder.Phone = order.Phone;
            newOrder.UserId = userId;
            newOrder.OrderStatus = OrderStatus.Processing;
            newOrder.PaymantMethod = radio;

            newOrder.InvoiceNo = RandomNumberServiceExtention.RandomString(6);
            newOrder.TrackingNo = RandomNumberServiceExtention.RandomStringAll(10);

            _context.Orders.Add(newOrder);
            _context.SaveChanges();


            foreach (var item in basketItems)
            {
                OrderItem newOrderitem = new OrderItem();

                newOrderitem.ProductId = item.ProductId;
                newOrderitem.OrderId = newOrder.Id;
                newOrderitem.Count = item.Count;
                newOrderitem.Price = item.Product.Price;
                newOrderitem.Total += item.Product.Price * item.Count;
                newOrder.TotalPrice += newOrderitem.Total;


                var dbproduct = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
                dbproduct.StockCount -= item.Count;

                _context.OrderItems.Add(newOrderitem);
                _context.BasketItems.Remove(item);
            }



            _context.SaveChanges();

            EmailService emailService = new EmailService(_config.GetSection("ConfirmationParams:Email").Value, _config.GetSection("ConfirmationParams:Password").Value);

            SendInovoice(newOrder.Id, order.Email, $"Invoice to {order.FirstName}", $"Thank you for being with us \n" +
                $"Your Tracking number: {newOrder.TrackingNo} ", $"{newOrder.InvoiceNo}.pdf");
            return RedirectToAction("index", "home");
        }

        public IActionResult OrderDetail(int? id)
        {
            if (id == null) return NotFound();

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.ProductImages)
                .FirstOrDefault(o => o.Id == id);

            if(order == null) return NotFound();

            return View(order);
        }

        public IActionResult Invoice(int id)
        {
            var order = _context.Orders
                .Include(u => u.User)
               .Include(o => o.OrderItems)
               .ThenInclude(o => o.Product)
               .FirstOrDefault(o => o.Id == id);

            return View(order);
        }

        public byte[] SendInovoice(int id,string email,string title,string body,string filename)
        {
            var mobileView = new HtmlToPdf();

            var fullView = new HtmlToPdf();
            fullView.Options.WebPageWidth = 1024;

            var pdf = fullView.ConvertUrl($"https://localhost:44330/order/invoice/{id}");

            var pdfBytes = pdf.Save();

            //using (var streamWriter = new StreamWriter(@"C:\Users\umudo\Desktop\Asp.net\BackendProject_Allup\BackendProject_Allup\wwwroot\Pdfs\pdf.pdf"))
            //{
            //    await streamWriter.BaseStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
            //}


            EmailService emailService = new EmailService(_config.GetSection("ConfirmationParams:Email").Value, _config.GetSection("ConfirmationParams:Password").Value);
            emailService.SendEmail(email, title, body, filename, pdfBytes);

            return pdfBytes;
        }


        //public string QrCodeGenerator(string inputText)
        //{
        //    string content = String.Empty;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
        //        QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(inputText, QRCodeGenerator.ECCLevel.Q);


        //        QRCode qRCode = new QRCode(qRCodeData);
        //        using (Bitmap bitmap = qRCode.GetGraphic(15))
        //        {
        //            bitmap.Save(ms, ImageFormat.Png);
        //            content = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return content;
        //}


    }
}
