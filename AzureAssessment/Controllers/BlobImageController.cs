using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class BlobImageController : Controller
	{
		public IActionResult Upload()
		{
			return View();
		}
	}
}
