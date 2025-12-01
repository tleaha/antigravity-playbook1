using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.Models
{
    public class Partner
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; } // In a real app, this should be hashed
        public string Name { get; set; }
        
        public ICollection<ReferralCode> ReferralCodes { get; set; }
        public ICollection<Commission> Commissions { get; set; }
    }
}
