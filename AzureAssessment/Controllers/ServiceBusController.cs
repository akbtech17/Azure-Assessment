using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class ServiceBusController : Controller
	{
		
		private static string? _connectionString;
		private static string? _serviceBusQueueName;
		private static ServiceBusClient _client;
		public ServiceBusController() {
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
			IConfiguration configuration = builder.Build();
			_connectionString = configuration.GetValue<string>("ConnectionStrings:ServiceBusQueueConnectionString");
			_serviceBusQueueName = configuration.GetValue<string>("ServiceBusQueueName");
			_client = new ServiceBusClient(_connectionString);
		}	
		public IActionResult UploadMessageToServiceBus()
		{
			ServiceBusSender _sender = _client.CreateSender(_serviceBusQueueName);
			ServiceBusMessage _message = new ServiceBusMessage("Anshul's first message to SBQ");
			_sender.SendMessageAsync(_message).GetAwaiter().GetResult();
			Console.WriteLine("Message Sent");
			return View();
		}
	}
}
