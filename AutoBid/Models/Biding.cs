using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoBid.Models
{
    public class Biding
    {
        [Key]
        public int BidId { get; set; }

        public int vehiucleId { get; set; }
        public String userId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BidAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
