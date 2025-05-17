using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace W31UL9_HSZF_2024252.Model
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Model { get; set; }
        public int TotalDistance { get; set; }
        public int DistanceSinceLastMaintenance { get; set; }

        //public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();

    }
}
