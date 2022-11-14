using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;

namespace AzureAssessment.Controllers
{
	public class ServiceBusController : Controller
	{
		
		private static string? _connectionString;
		private static string? _serviceBusQueueName;
		private static ServiceBusClient? _client;
		public ServiceBusController() {
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
			IConfiguration configuration = builder.Build();
			_connectionString = configuration.GetValue<string>("ConnectionStrings:ServiceBusQueueConnectionString");
			_serviceBusQueueName = configuration.GetValue<string>("ServiceBusQueueName");
			_client = new ServiceBusClient(_connectionString);
		}	
		public IActionResult UploadMessageToServiceBus()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UploadMessageToServiceBus(Message message)
		{
			ServiceBusSender _sender = _client.CreateSender(_serviceBusQueueName);
			ServiceBusMessage _message = new ServiceBusMessage(message.MessageString);
			_sender.SendMessageAsync(_message).GetAwaiter().GetResult();
			Console.WriteLine("Message Sent");
			return View();
		}

		[HttpGet]
		public IActionResult RecieveMessageFromServiceBus() 
		{
			RecieveMessages obj = new RecieveMessages();
			return View(obj);
		}

		[HttpPost]
		public IActionResult RecieveMessageFromServiceBus(RecieveMessages request) 
		{
			ServiceBusReceiver _reciever = _client.CreateReceiver(_serviceBusQueueName, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });
			var _messages = _reciever.ReceiveMessagesAsync(request.MessageCount).GetAwaiter().GetResult();
			request.Messages = new List<string>();
			foreach (var _message in _messages)
			{
				request.Messages.Add(_message.Body.ToString());
			}
			return View(request);
		}
	}
}
