using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class ServiceBusController : Controller
	{
		public IActionResult UploadMessageToServiceBus()
		{
			return View();
		}
	}
}
