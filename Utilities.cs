using SmartRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SmartRentalApp
{
    public class Utilities
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static SmartRentalDBEntities realEstateDB = new SmartRentalDBEntities();

        public static ApplicationUser GetLoggedInUser
        {
            get
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                    return db.Users.Where(u => u.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefault();
                else
                    return null;
            }
        }

        public static bool IsDistrictFavorite(string username, int districtID)
        {
            var result = realEstateDB.CustomersFavorites.Where(f => f.DistrictID == districtID && f.Username == username).FirstOrDefault();

            return result != null;
        }

        public static void SendSubscriptionEmail(int districtID, long realEstateID)
        {
            var users = realEstateDB.CustomersFavorites.Where(c => c.DistrictID == districtID).Select(u => u.Username).ToList();

            foreach (var user in users)
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("smart.rental.guide@gmail.com", "P@ssw0rd@123");

                using (var message = new MailMessage(
                    new MailAddress("smart.rental.guide@gmail.com", "Smart Rental Guide"),
                    new MailAddress(user, "Dear Customer")))
                {
                    message.Subject = "New Real Estate added in you subscription district";
                    message.Body = "<b>The following real estate has been added in you subscription district:</b>";
                    message.Body += "<br/><br/><a href='" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Home/Details/" + realEstateID + "'>Click here to view the details of Real Estate</a>";
                    message.Body += "</br></br><b>Smart Rental Guide</b>";
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
        }
    }
}