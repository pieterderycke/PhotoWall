using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoWall.Models
{
    public interface IUserManager
    {
        string AddUserImage(string userName, byte[] imageBytes);

        IEnumerable<PhotoInformation> GetUserImages();

        byte[] GetUserImageData(string userName);
    }
}