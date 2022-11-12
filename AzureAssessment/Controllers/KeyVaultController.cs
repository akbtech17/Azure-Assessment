using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class KeyVaultController : Controller
	{
		private static string? _tenantId;
		private static string? _clientId;
		private static string? _clientSecret;

        private static string? _keyVaultUrl;
		private static SecretClient? _secretsClient;
		//private static string? _sqlConnString;
        public KeyVaultController(IConfiguration configuration) {
			_tenantId = configuration["TenantId"];
			_clientId = configuration["ClientId"];
			_clientSecret = configuration["ClientSecret"];

			ClientSecretCredential clientCredential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);

            _keyVaultUrl = configuration["AzureKeyVaultUrl"];
            //_secretsClient = new SecretClient(new Uri(_keyVaultUrl), new DefaultAzureCredential());
            _secretsClient = new SecretClient(new Uri(_keyVaultUrl), clientCredential);
            var _sqlConnString = _secretsClient.GetSecret("AdminPassword");
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
