using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Messaging.ServiceBus;
using AzureAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAssessment.Controllers
{
	public class ServiceBusController : Controller
	{
		private static string? connectionString;
		private static string? serviceBusQueueName;
		private static ServiceBusClient? client;
        private readonly INotyfService? notyf;
		private static ServiceBusReceiver? reciever;

        public ServiceBusController(IConfiguration configuration, INotyfService notyf) {
			this.notyf = notyf;
			connectionString = configuration.GetValue<string>("ConnectionStrings:ServiceBusQueueConnectionString");
			serviceBusQueueName = configuration.GetValue<string>("ServiceBusQueueName");
			client = new ServiceBusClient(connectionString);
            reciever = client.CreateReceiver(serviceBusQueueName, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });
        }	
		public IActionResult UploadMessageToServiceBus()
		{
			return View();
        }

		[HttpPost]
		public IActionResult UploadMessageToServiceBus(Message messageModel)
		{
			try
			{
				if (client != null) {
                    ServiceBusSender sender = client.CreateSender(serviceBusQueueName);
                    ServiceBusMessage message = new ServiceBusMessage(messageModel.MessageString);
                    sender.SendMessageAsync(message).GetAwaiter().GetResult();
                    Console.WriteLine("Message Sent");
                    notyf?.Success("Message sent successfully!");
                }
            }
			catch(Exception ex) 
			{
				Console.WriteLine(ex.Message);
				notyf?.Error(ex.InnerException == null ? "" : ex.InnerException.Message);
			}
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
            try
            {
                if (reciever != null)
                {
                    var messages = reciever.ReceiveMessagesAsync(request.MessageCount).GetAwaiter().GetResult();
                    request.Messages = new List<string>();
                    foreach (var message in messages)
                    {
                        request.Messages.Add(message.Body.ToString());
                    }
                    if (request.Messages.Count == 0) notyf?.Information("Sorry there are 0 messages in queue!");
                    else notyf?.Success($"Fetched {request.Messages.Count} messages!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                notyf?.Error(ex.InnerException == null ? "" : ex.InnerException.Message);
            }
            return View(request);
        }
    }
}
