using System.ComponentModel.DataAnnotations;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;

namespace AzureAssessment.Models
{
	public class RecieveMessages
	{
        [Required(ErrorMessage = "* message can't be empty")]
        [Range(1, 10, ErrorMessage = "* enter values betweeen 1-10")]
        public int MessageCount { get; set; }
		public List<string>? Messages { get; set; }
	}
}
