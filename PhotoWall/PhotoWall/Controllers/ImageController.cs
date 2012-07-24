using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoWall.Models;

namespace PhotoWall.Controllers
{
    public class ImageController : Controller
    {
        private readonly IUserManager userManager;

        public ImageController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public ActionResult Index(string userName)
        {
            byte[] bytes = userManager.GetUserImageData(userName);
            return File(bytes, "image/jpeg");
        }

    }
}
