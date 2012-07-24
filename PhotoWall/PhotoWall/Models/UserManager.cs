using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using PhotoWall.Models;

namespace PhotoWall.Models
{
    public class UserManager : IUserManager
    {
        private const int Width = 320;
        private const int Height = 240;

        private readonly string snapshotDirectory;
        private readonly Dictionary<string, PhotoInformation> photos;

        public UserManager()
        {
            this.photos = new Dictionary<string, PhotoInformation>();

            string applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            this.snapshotDirectory = Path.Combine(applicationPath, "Snapshots");

            if (!Directory.Exists(this.snapshotDirectory))
                Directory.CreateDirectory(snapshotDirectory);
        }

        public string AddUserImage(string userName, byte[] imageBytes)
        {
            if(string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");

            string imageUrl = SaveImage(userName, imageBytes);

            return imageUrl;
        }

        public IEnumerable<PhotoInformation> GetUserImages()
        {
            IEnumerable<string> snapshots = Directory.EnumerateFiles(snapshotDirectory, "snapshot_*.png");

            IList<PhotoInformation> photos = new List<PhotoInformation>();

            foreach(string snapshot in snapshots)
            {
                string fileName = Path.GetFileNameWithoutExtension(snapshot);
                string userName = fileName.Substring(fileName.LastIndexOf('_') + 1);
                string imageUrl = GetImageUrl(userName);

                photos.Add(new PhotoInformation() { UserName = userName, PhotoUrl = imageUrl });
            }

            return photos;
        } 

        public byte[] GetUserImageData(string userName)
        {
            string path = Path.Combine(snapshotDirectory, string.Format("snapshot_{0}.jpg", userName));
            return System.IO.File.ReadAllBytes(path);
        }

        private string SaveImage(string userName, byte[] imageBytes)
        {
            Bitmap image = ResizeImage(imageBytes);

            int snapshotCount = Directory.GetFiles(snapshotDirectory).Count() + 1;

            string path = Path.Combine(snapshotDirectory, string.Format("snapshot_{0}.jpg", userName));
            image.Save(path, ImageFormat.Jpeg);

            return "/Image?userName=" + userName;
        }

        private Bitmap ResizeImage(byte[] imageBytes)
        {
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                Bitmap inputImage = new Bitmap(stream);
                Bitmap outputImage = new Bitmap(Width, Height);

                using(Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(inputImage, 0, 0, Width, Height);
                }

                return outputImage;
            }
        }

        private string GetImageUrl(string userName)
        {
            return "/Image?userName=" + userName;
        }
    }
}