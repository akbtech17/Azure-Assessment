using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
                    string imgCaption = file.FileCaption == null ? "" : file.FileCaption;

                    UploadCSVFile(myFile);
                }
				
				notyf?.Success("File uploaded successfully!");
                return RedirectToAction("List");
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				notyf?.Error(ex.InnerException == null ? "" : ex.InnerException.Message);
			}
			return View();
		}

		[HttpGet]
		public IActionResult List()
		{
            List<CSVFile> csvs = new List<CSVFile>();
            try
            {
                if (containerClient == null)
                {
                    notyf?.Error("Internal Error Occurred!");
                    return View(csvs);
                }

                foreach (BlobItem item in containerClient.GetBlobs())
                {
                    csvs.Add(new CSVFile { FileName = item.Name});
                }

            }
            catch (Exception ex)
            {
                notyf?.Error("Internal Error Occurred!");
                Console.WriteLine(ex.Message);
            }
            return View(csvs);
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
