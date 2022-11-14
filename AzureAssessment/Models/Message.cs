using System.ComponentModel.DataAnnotations;

namespace AzureAssessment.Models
{
	public class Message
	{
        [Required(ErrorMessage = "* message can't be empty")]
        public string? MessageString { get; set; }
		public int? MessageCount { get; set; }	
	}
}
