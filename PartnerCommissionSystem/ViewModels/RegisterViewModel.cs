using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        public string Role { get; set; } // "Partner" or "BusinessOwner"
    }
}
