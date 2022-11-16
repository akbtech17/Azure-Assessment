using System.ComponentModel.DataAnnotations;

namespace AzureAssessment.Models
{
    public class KeyVault
    {
        [Required(ErrorMessage = "* key can't be empty")]
        public string? Key { get; set; }

        [Required(ErrorMessage = "* value can't be empty")]
        public string? Value { get; set; }
    }
}
