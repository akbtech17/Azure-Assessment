using AspNetCoreHero.ToastNotification.Abstractions;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobImageController : Controller
	{
		private static string? containerName;
		private static string? connectionString;
		private static string? _imageBaseUrl;
        BlobServiceClient? serviceClient;
		BlobContainerClient? containerClient;
        private readonly INotyfService? notyf;

        public BlobImageController(IConfiguration configuration, INotyfService notyf)
		{
			try
			{
				this.notyf = notyf;
				connectionString = configuration.GetValue<string>("ConnectionStrings:BlobConnectionString");
				containerName = configuration.GetValue<string>("BlobContainerName");
				_imageBaseUrl = configuration.GetValue<string>("BlobImagesBaseUrl");

				serviceClient = new BlobServiceClient(connectionString);
				containerClient = serviceClient.GetBlobContainerClient(containerName);
                
            }
			catch(Exception ex) 
			{
				Console.WriteLine(ex.Message);
			}
		}

		[HttpGet]
		public IActionResult UploadImageToBlob()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UploadImageToBlob(Image imageInfo) 
		{
			try
			{
				if (imageInfo != null && imageInfo.MyImage != null)
				{
					IFormFile image = imageInfo.MyImage;
					string imgCaption = imageInfo.ImageCaption == null ? "" : imageInfo.ImageCaption;
					string imgDescription = imageInfo.ImageDescription == null ? "" : imageInfo.ImageDescription;

					UploadImageBlob(image, imgCaption, imgDescription);
					notyf?.Success("Image Uploaded Successfully!");
					return RedirectToAction("ImageList");
				}
				else {
                    notyf?.Error("Internal Error Occurred!");
                }
            }
			catch (Exception ex)
			{
                notyf?.Error("Internal Error Occurred!");
                Console.WriteLine(ex.Message);
			}
			return View();
		}

		[HttpGet]
		public IActionResult ImageList() 
		{
            List<Image> images = new List<Image>();
            try
			{
				if (containerClient == null) {
                    notyf?.Error("Internal Error Occurred!");
                    return View(images);
                }

				foreach (BlobItem item in containerClient.GetBlobs())
				{
					images.Add(new Image { ImageName = item.Name, ImageUrl = _imageBaseUrl + item.Name });
				}
                
            }
			catch (Exception ex) 
			{
                notyf?.Error("Internal Error Occurred!");
                Console.WriteLine(ex.Message);
            }
            return View(images);
        }

		public void UploadImageBlob(IFormFile image, string caption, string descritpion)
		{
			if (containerClient == null) return;
			BlobClient blobClient = containerClient.GetBlobClient(image.FileName);
			using (Stream stream = image.OpenReadStream()) 
				blobClient.Upload(stream);

			IDictionary<string, string> metadata = new Dictionary<string, string>();
			if (caption != null) metadata.Add("Caption", caption);
			if (descritpion != null) metadata.Add("Description", descritpion);
			blobClient.SetMetadata(metadata);
			Console.WriteLine($"{image.FileName} has been uploaded to {containerName} conatainer of Blob Storage");   
		}
	}
}
