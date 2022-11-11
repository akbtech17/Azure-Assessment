using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AzureAssessment.Controllers
{
	public class BlobImageController : Controller
	{
		[Obsolete]
		private readonly IHostingEnvironment _environment;
		private static string? _containerName;
		private static string? _connectionString;
		BlobServiceClient _serviceClient;
		BlobContainerClient _containerClient;

		[Obsolete]
		public BlobImageController(IHostingEnvironment environment)
		{
			_environment = environment;
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
			if (imageInfo !=null && imageInfo.MyImage != null) {
				string fileName = Path.GetFileName(imageInfo.MyImage.FileName);			
				string filePath = "C:\\Users\\10711704\\Downloads";
				filePath = Path.Combine(filePath, fileName);	

				/*IFormFile img = imageInfo.MyImage;
				string? imgCaption = imageInfo.ImageCaption;*/

				string? contentType = imageInfo.MyImage.ContentType;
				BlobClient blobClient = _containerClient.GetBlobClient(fileName);
				blobClient.Upload(filePath);
				Console.WriteLine("New blob is created");
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
	}
}
