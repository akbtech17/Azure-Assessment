using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobCSVController : Controller
	{
		[HttpGet]
		public IActionResult UploadCSVToBlob()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UploadCSVToBlob(Image imageInfo)
		{
			var img = imageInfo.MyImage;
			var imgCaption = imageInfo.ImageCaption;

			//Getting file meta data
			var fileName = Path.GetFileName(imageInfo.MyImage.FileName);
			var contentType = imageInfo.MyImage.ContentType;
			return View();
		}
	}
}
