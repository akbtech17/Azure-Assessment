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
	}
}
