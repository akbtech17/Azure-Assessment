namespace AzureAssessment.Models
{
	public class CSVFile
	{
		public string? FileCaption { get; set; }
		public string? FileDescription { get; set; }
		public IFormFile? MyFile { get; set; }
		public string? FileName { get; set; }
		public string? FileUrl { get; set; }
	}
}
