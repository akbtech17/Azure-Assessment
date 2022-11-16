using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using AzureAssessment.Models;
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

        public KeyVaultController(IConfiguration configuration) {
			_tenantId = configuration["TenantId"];
			_clientId = configuration["ClientId"];
			_clientSecret = configuration["ClientSecret"];
            _keyVaultUrl = configuration["AzureKeyVaultUrl"];

			ClientSecretCredential clientCredential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            _secretsClient = new SecretClient(new Uri(_keyVaultUrl), clientCredential);
            
		}

		[HttpGet]
		public IActionResult Index()
		{
			KeyVault keyVaultModel = new KeyVault();
			return View(keyVaultModel);
		}

		[HttpPost]
		public IActionResult Index(KeyVault keyVaultModel)  
		{
			if (keyVaultModel != null && keyVaultModel.Key != null)  {
				keyVaultModel.Value = _secretsClient?.GetSecret(keyVaultModel.Key).Value.Value;
            }
			return View(keyVaultModel);
		}

		[HttpGet]
		public IActionResult CreateSecret() 
		{
			return View();
		}

		[HttpPost]
		public IActionResult CreateSecret(KeyVault keyVaultModel) 
		{
			string key = keyVaultModel.Key;
			string value = keyVaultModel.Value;
			KeyVaultSecret keyVaultSecret = _secretsClient.SetSecret(key, value);
            return View();
		}
	}
}
