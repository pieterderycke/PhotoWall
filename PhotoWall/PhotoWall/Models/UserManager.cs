using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PhotoWall.Models;

namespace PhotoWall.Models
{
    public class UserManager : IUserManager
    {
        private const string SnapshotDirectory = @"C:\Temp\Html5 Video";

        private readonly Dictionary<string, PhotoInformation> photos;

        public UserManager()
        {
            photos = new Dictionary<string, PhotoInformation>();
        }

        public string AddUserPhoto(string userName, byte[] imageBytes)
        {
            if(string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");

            string imageUrl = SavePhoto(userName, imageBytes);
            //photos.Add(userName, new PhotoInformation() { UserName = userName, PhotoUrl = imageUrl });

            return imageUrl;
        }

        public IEnumerable<PhotoInformation> GetUserPhotos()
        {
            IEnumerable<string> snapshots = Directory.EnumerateFiles(SnapshotDirectory, "snapshot_*.png");

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

        private string SavePhoto(string userName, byte[] imageBytes)
        {
            int snapshotCount = Directory.GetFiles(SnapshotDirectory).Count() + 1;

            string path = Path.Combine(SnapshotDirectory, string.Format("snapshot_{0}.png", userName));
            System.IO.File.WriteAllBytes(path, imageBytes);

            return "/Image?userName=" + userName;
        }

        private string GetImageUrl(string userName)
        {
            return "/Image?userName=" + userName;
        }
    }
}