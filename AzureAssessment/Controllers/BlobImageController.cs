using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobImageController : Controller
	{
		public IActionResult UploadImageToBlob()
		{
			return View();
		}

		public IActionResult ImageList() 
		{
			return View();
		}
	}
}
