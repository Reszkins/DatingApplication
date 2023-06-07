using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace API.Services
{
    public class PhotoService
    {

        public const string PhotosFolder = "Photos";
        public readonly IWebHostEnvironment _webHostEnvironment;

        public PhotoService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task SavePhoto(Stream photoStream, int userId)
        {
            string baseFolderPath = _webHostEnvironment.ContentRootPath;
            string photosFolderPath = Path.Combine(baseFolderPath, PhotosFolder);
            string userFolder = Path.Combine(photosFolderPath, userId.ToString());

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
                Console.WriteLine($"User folder created: {userFolder}");
            } else
            {
                Console.WriteLine("Error creating user folder");
            }

            // string fileName = $"{Guid.NewGuid()}.jpg";
            ///   string filePath = Path.Combine(userFolder, fileName);

            // using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            //  {
            //      await photoStream.CopyToAsync(fileStream);
            // }
            string fileName = $"{Guid.NewGuid()}.txt";
            string filePath = Path.Combine(userFolder, fileName);

            using (StreamReader reader = new StreamReader(photoStream))
            {
                byte[] fileBytes = new byte[photoStream.Length];
                await photoStream.ReadAsync(fileBytes, 0, (int)photoStream.Length);
                string base64String = Convert.ToBase64String(fileBytes);

                await System.IO.File.WriteAllTextAsync(filePath, base64String);
                Console.Write(base64String);
            }
        }
    }
}

