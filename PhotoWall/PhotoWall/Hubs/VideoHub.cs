using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoWall.Models;
using SignalR.Hubs;
using PhotoWall.Controllers;

namespace PhotoWall.Hubs
{
    [HubName("videoHub")]
    public class VideoHub : Hub
    {
        private readonly IUserManager userManager;

        public VideoHub(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public void Send(string userName, string data)
        {
            byte[] imageBytes = ExtractImageFromRequest(data);
            string imageUrl = userManager.AddUserPhoto(userName, imageBytes);

            Clients.display(userName, imageUrl);
        }

        private byte[] ExtractImageFromRequest(string requestData)
        {
            string[] image = requestData.Split(',');
            return Convert.FromBase64String(image[1]);
        }
    }
}