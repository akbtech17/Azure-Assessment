using System.ComponentModel.DataAnnotations;

namespace AzureAssessment.Models
{
	public class Image
	{
        [Required(ErrorMessage = "*caption is required")]
        public string? ImageCaption { get; set; }

        [Required(ErrorMessage = "*description is required")]
        public string? ImageDescription { get; set; }

        [Required(ErrorMessage = "*select image")]
        public IFormFile? MyImage { get; set; }
		public string? ImageName { get; set; }
		public string? ImageUrl { get; set; }
	}
}
