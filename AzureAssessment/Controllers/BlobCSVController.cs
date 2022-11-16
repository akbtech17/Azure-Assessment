using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobCSVController : Controller
	{
		private static string _containerName = "data";
		private static string? _connectionString;
		BlobServiceClient _serviceClient;
		BlobContainerClient _containerClient;

		public BlobCSVController()
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
			IConfiguration configuration = builder.Build();
			_connectionString = configuration.GetValue<string>("ConnectionStrings:BlobConnectionString");

			_serviceClient = new BlobServiceClient(_connectionString);
			_containerClient = _serviceClient.GetBlobContainerClient(_containerName);
		}
		[HttpGet]
		public IActionResult UploadCSVToBlob()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UploadCSVToBlob(CSVFile file)
		{
			if (file != null && file.MyFile != null)
			{
				IFormFile myFile = file.MyFile;
				string? imgCaption = file.FileCaption;
				string? contentType = file.MyFile.ContentType;

				UploadCSVFile(myFile);
			}
			return View();
		}

		[HttpGet]
		public IActionResult List()
		{
			return View();
		}

		public void UploadCSVFile(IFormFile file)
		{
			BlobClient blobClient = _containerClient.GetBlobClient(file.FileName);
			using (var stream = file.OpenReadStream())
			{
				blobClient.Upload(stream);
			}
			Console.WriteLine($"{file.FileName} has been uploaded to {_containerName} conatainer of Blob Storage");
		}
	}
}
