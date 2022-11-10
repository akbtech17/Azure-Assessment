using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class KeyVaultController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
