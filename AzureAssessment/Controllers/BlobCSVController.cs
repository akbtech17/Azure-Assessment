using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobCSVController : Controller
	{
		public IActionResult UploadCSVToBlob()
		{
			return View();
		}
	}
}
