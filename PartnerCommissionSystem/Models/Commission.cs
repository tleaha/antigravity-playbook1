using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.Models
{
    public class Commission
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }
        
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
