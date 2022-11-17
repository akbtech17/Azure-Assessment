using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class KeyVaultController : Controller
	{
		private static string? tenantId;
		private static string? clientId;
		private static string? clientSecret;
        private static string? keyVaultUrl;
		private static SecretClient? secretsClient;
        private readonly INotyfService? notyf;

        public KeyVaultController(IConfiguration configuration, INotyfService notyf) {
			this.notyf = notyf;

			tenantId = configuration["TenantId"];
			clientId = configuration["ClientId"];
			clientSecret = configuration["ClientSecret"];
            keyVaultUrl = configuration["AzureKeyVaultUrl"];

			ClientSecretCredential clientCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            secretsClient = new SecretClient(new Uri(keyVaultUrl), clientCredential);
            
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
			try
			{
                if (keyVaultModel != null && keyVaultModel.Key != null)
                {
                    keyVaultModel.Value = secretsClient?.GetSecret(keyVaultModel.Key).Value.Value;
                }

				notyf?.Success("Found the secret!");
            }
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
                notyf?.Error($"Error : {ex.InnerException?.Message}");
            }
            return View(keyVaultModel);

        }

		[HttpGet]
		public IActionResult SetSecret() 
		{
			return View();
		}

		[HttpPost]
		public IActionResult SetSecret(KeyVault keyVaultModel) 
		{
			try 
			{
				if (keyVaultModel != null && secretsClient != null) 
				{
                    string key = keyVaultModel.Key == null ? "" : keyVaultModel.Key;
                    string value = keyVaultModel.Value == null ? "" : keyVaultModel.Value;
                    KeyVaultSecret keyVaultSecret = secretsClient.SetSecret(key, value);
                    notyf?.Success("Successfully created the secret!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                notyf?.Error($"Error : {ex.InnerException?.Message}");
            }
            return View();
		}
	}
}
