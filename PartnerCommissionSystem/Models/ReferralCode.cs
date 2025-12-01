using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.Models
{
    public class ReferralCode
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }
    }
}
