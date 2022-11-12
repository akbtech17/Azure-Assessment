using System.ComponentModel.DataAnnotations;

namespace AzureAssessment.Models
{
	public class RecieveMessages
	{
        [Required(ErrorMessage = "* message can't be empty")]
        public int MessageCount { get; set; }
		public List<string>? Messages { get; set; }
	}
}
