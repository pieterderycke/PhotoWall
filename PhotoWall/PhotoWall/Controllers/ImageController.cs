using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoWall.Controllers
{
    public class ImageController : Controller
    {
        private const string SnapshotDirectory = @"C:\Temp\Html5 Video";

        public ActionResult Index(string userName)
        {
            string path = Path.Combine(SnapshotDirectory, string.Format("snapshot_{0}.png", userName));
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "image/png");
        }

    }
}
