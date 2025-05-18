using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W31UL9_HSZF_2024252.Model
{
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public decimal Distance { get; set; }
        public decimal Cost { get; set; }
    }
}
