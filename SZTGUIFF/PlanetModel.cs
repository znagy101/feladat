using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Models
{
    public class PlanetModel
    {
        public string Name { get; set; } = string.Empty;
        public double OrbitRadius { get; set; }
        public double Size { get; set; }    
        public double OrbitSpeed { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public PlanetModel(string Name, double OrbitRadius, double Size, double OrbitSpeed, string ImagePath )
        {
            this.Name = Name;
            this.OrbitRadius = OrbitRadius;
            this.Size = Size;
            this.OrbitSpeed = OrbitSpeed;
            this.ImagePath = ImagePath;
        }


    }
}
