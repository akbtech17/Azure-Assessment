using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Storage.Blobs;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobCSVController : Controller
	{
		private static string containerName = "data";
		private static string? connectionString;
		BlobServiceClient serviceClient;
		BlobContainerClient containerClient;
        private readonly INotyfService? notyf;

        public BlobCSVController(IConfiguration configuration, INotyfService notyf)
		{
			this.notyf = notyf;
			connectionString = configuration.GetValue<string>("ConnectionStrings:BlobConnectionString");
			serviceClient = new BlobServiceClient(connectionString);
			containerClient = serviceClient.GetBlobContainerClient(containerName);
		}
		[HttpGet]
		public IActionResult UploadCSVToBlob()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UploadCSVToBlob(CSVFile file)
		{
			try 
			{
                if (file != null && file.MyFile != null)
                {
                    IFormFile myFile = file.MyFile;
                    string? imgCaption = file.FileCaption;
                    string? contentType = file.MyFile.ContentType;

                    UploadCSVFile(myFile);
                }
				notyf.Success("File uploaded successfully!");
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				notyf.Error(ex.InnerException.Message));
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
			BlobClient blobClient = containerClient.GetBlobClient(file.FileName);
			using (Stream stream = file.OpenReadStream())
			{
				blobClient.Upload(stream);
			}
			Console.WriteLine($"{file.FileName} has been uploaded to {containerName} conatainer of Blob Storage");
		}
	}
}
