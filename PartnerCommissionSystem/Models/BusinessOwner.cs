using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.Models
{
    public class BusinessOwner
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; } // In a real app, this should be hashed
        public string Name { get; set; }

        public ICollection<Accommodation> Accommodations { get; set; }
    }
}
