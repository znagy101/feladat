using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace W31UL9_HSZF_2024252.Model
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Model { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal DistanceSinceLastMaintenance { get; set; }

        //public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();

    }
}
