using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AzureAssessment.Controllers
{
	public class BlobImageController : Controller
	{
		private static string? _containerName;
		private static string? _connectionString;
		BlobServiceClient _serviceClient;
		BlobContainerClient _containerClient;

		public BlobImageController()
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
			IConfiguration configuration = builder.Build();
			_connectionString = configuration.GetValue<string>("ConnectionStrings:BlobConnectionString");
			_containerName = configuration.GetValue<string>("BlobContainerName");

			_serviceClient = new BlobServiceClient(_connectionString);
			_containerClient = _serviceClient.GetBlobContainerClient(_containerName);
		}

		[HttpGet]
		public IActionResult UploadImageToBlob()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UploadImageToBlob(Image imageInfo) 
		{
			if (imageInfo != null && imageInfo.MyImage != null) 
			{
				IFormFile image = imageInfo.MyImage;
				string? imgCaption = imageInfo.ImageCaption;
				string? contentType = imageInfo.MyImage.ContentType;

				UploadImageBlob(image);
			}
			return View();
		}

		public IActionResult ImageList() 
		{
			List<Image> images = new List<Image>();
			foreach (BlobItem item in _containerClient.GetBlobs())
			{
				images.Add(new Image { ImageName = item.Name, ImageUrl = "https://storageaccount94111.blob.core.windows.net/images/"+item.Name });
			}
			return View(images);
		}

		public void UploadImageBlob(IFormFile image)
		{
			BlobClient blobClient = _containerClient.GetBlobClient(image.FileName);
			using (var stream = image.OpenReadStream())
			{
				blobClient.Upload(stream);
			}
			Console.WriteLine($"{image.FileName} has been uploaded to {_containerName} conatainer of Blob Storage");
		}
	}
}
