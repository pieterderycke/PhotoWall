using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using PhotoWall.Models;

namespace PhotoWall.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManager userManager;

        public HomeController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPhotos()
        {
            return Json(userManager.GetUserPhotos());
        }

        private byte[] ExtractImageFromRequest()
        {
            byte[] bytes = Request.BinaryRead(Request.TotalBytes);
            string data = Encoding.UTF8.GetString(bytes);

            string[] image = data.Split(',');
            return Convert.FromBase64String(image[1]);
        }
    }
}
