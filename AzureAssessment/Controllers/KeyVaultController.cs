using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class KeyVaultController : Controller
	{
		private static string? _keyVaultUrl;
		private static SecretClient? _secretsClient;
		//private static string? _sqlConnString;
        public KeyVaultController(IConfiguration configuration) {
			_keyVaultUrl = configuration["AzureKeyVaultUrl"];
			_secretsClient = new SecretClient(new Uri(_keyVaultUrl), new DefaultAzureCredential());
			var _sqlConnString = _secretsClient.GetSecret("AdminPassword");
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
