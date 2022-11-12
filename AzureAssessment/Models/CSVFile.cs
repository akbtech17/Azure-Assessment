using System.ComponentModel.DataAnnotations;

namespace AzureAssessment.Models
{
	public class CSVFile
	{
        [Required(ErrorMessage = "* caption is required")]
        public string? FileCaption { get; set; }
        [Required(ErrorMessage = "* description is required")]
        public string? FileDescription { get; set; }

        [Required(ErrorMessage = "* please select the file")]
        public IFormFile? MyFile { get; set; }
		public string? FileName { get; set; }
		public string? FileUrl { get; set; }
	}
}
