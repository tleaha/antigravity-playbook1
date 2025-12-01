using System.ComponentModel.DataAnnotations;

namespace PartnerCommissionSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal Amount { get; set; }
        
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
    }
}
