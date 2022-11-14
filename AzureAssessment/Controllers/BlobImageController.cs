﻿using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobImageController : Controller
	{
		private static string? _containerName;
		private static string? _connectionString;
		private static string? _imageBaseUrl;
        BlobServiceClient? _serviceClient;
		BlobContainerClient? _containerClient;

		public BlobImageController(IConfiguration configuration)
		{
			try
			{ 
                _connectionString = configuration.GetValue<string>("ConnectionStrings:BlobConnectionString");
                _containerName = configuration.GetValue<string>("BlobContainerName");
				_imageBaseUrl = configuration.GetValue<string>("BlobImagesBaseUrl");

                _serviceClient = new BlobServiceClient(_connectionString);
                _containerClient = _serviceClient.GetBlobContainerClient(_containerName);
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
			if (imageInfo != null && imageInfo.MyImage != null) 
			{
				IFormFile image = imageInfo.MyImage;
				string? imgCaption = imageInfo.ImageCaption;
				string? imgDescription = imageInfo.ImageDescription;

				UploadImageBlob(image,imgCaption, imgDescription);
			}
			return View();
		}

		public IActionResult ImageList() 
		{
			List<Image> images = new List<Image>();
			if (_containerClient == null) return View(images);

            foreach (BlobItem item in _containerClient.GetBlobs())
			{
                images.Add(new Image { ImageName = item.Name, ImageUrl = _imageBaseUrl+item.Name});
			}
			return View(images);
		}

		public void UploadImageBlob(IFormFile image, string? caption, string? descritpion)
		{
			try 
			{
                BlobClient? blobClient = _containerClient?.GetBlobClient(image.FileName);
                using (var stream = image.OpenReadStream()) blobClient?.Upload(stream);

                IDictionary<string, string> metadata = new Dictionary<string, string>();
                if (caption != null) metadata.Add("Caption", caption);
                if (descritpion != null) metadata.Add("Description", descritpion);
                blobClient?.SetMetadata(metadata);
                Console.WriteLine($"{image.FileName} has been uploaded to {_containerName} conatainer of Blob Storage");
            }
			catch (RequestFailedException ex) {
				Console.WriteLine($"HTTP error code {ex.Status}: {ex.ErrorCode}");
				Console.WriteLine(ex.Message);
            }
		}
	}
}
