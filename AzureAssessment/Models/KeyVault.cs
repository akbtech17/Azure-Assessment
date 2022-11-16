using System.ComponentModel.DataAnnotations;

namespace AzureAssessment.Models
{
    public class KeyVault
    {
        [Required(ErrorMessage = "* key can't be empty")]
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
