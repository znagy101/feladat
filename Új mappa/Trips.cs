using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace W31UL9_HSZF_2024252.Model.EntityesLists
{
    [XmlRoot("Trips")]
    public class Trips
    {
        [XmlElement("Trip")]
        public List<Trip> TripList { get; set; }
    }
}
