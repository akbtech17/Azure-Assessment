namespace AzureAssessment.Models
{
	public class UploadImage
	{
		public string? ImageCaption { get; set; }
		public string? ImageDescription { get; set; }
		public IFormFile? MyImage { get; set; }
	}
}
