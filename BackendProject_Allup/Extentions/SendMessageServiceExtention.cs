using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;

namespace BackendProject_Allup.Extentions
{
    public static class SendMessageServiceExtention
    {
        public static async Task SendAllUsersNewProduct(List<AppUser>users,Product product, IConfiguration config)
        {
            foreach (var user in users)
            {

                EmailService emailService = new EmailService(config.GetSection("ConfirmationParams:Email").Value, config.GetSection("ConfirmationParams:Password").Value);
                emailService.SendEmail(user.Email, $"New product added", $"{product.Name} {product.Price}");
            }
        } 
    }
}

