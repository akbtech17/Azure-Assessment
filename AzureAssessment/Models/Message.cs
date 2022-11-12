using System.ComponentModel.DataAnnotations;

namespace AzureAssessment.Models
{
	public class Message
	{
        [Required(ErrorMessage = "* please select image")]
        public string? MessageString { get; set; }
		public int? MessageCount { get; set; }	
	}
}
