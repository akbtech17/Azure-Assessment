namespace AzureAssessment.Models
{
	public class Image
	{
		public string? ImageCaption { get; set; }
		public string? ImageDescription { get; set; }
		public IFormFile? MyImage { get; set; }
		public string? ImageName { get; set; }
		public string? ImageUrl { get; set; }
	}
}
