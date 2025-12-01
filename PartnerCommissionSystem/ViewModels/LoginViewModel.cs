using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; } // "Partner" or "BusinessOwner"
    }
}
