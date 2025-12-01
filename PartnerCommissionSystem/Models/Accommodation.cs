using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public int BusinessOwnerId { get; set; }
        public BusinessOwner BusinessOwner { get; set; }

        public string? ReferralCode { get; set; } // Stores the code string used during registration
    }
}
