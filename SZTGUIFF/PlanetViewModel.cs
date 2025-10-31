using CommunityToolkit.Mvvm.ComponentModel;
using SolarSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SolarSystem.ViewModels
{
    public partial class PlanetViewModel : ObservableObject
    {
        //[ObservableProperty]
        public ObservableCollection<PlanetModel> Planets { get; } = new(); 
        
        

        public PlanetViewModel()
        {
            Planets = new ObservableCollection<PlanetModel>
            {
                new PlanetModel("Sun", 25,100,0, "/Sources/Images/Sun.jpeg") ,
                new PlanetModel("Mercury", 15.0, 16.0, 4.09, "/Sources/Images/Mercury.jpeg"),//opti30   base50       size50
                new PlanetModel("Venus", 20.0, 24.0, 1.60, "/Sources/Images/Venus.jpeg"), //40  80      8
                new PlanetModel("Earth", 30.0, 24.0, 0.986, "/Sources/Images/Earth.jpeg"), // 60   120      12
                new PlanetModel("Mars", 20.0, 20.0, 0.524, "/Sources/Images/Mars.jpeg"), //40  180      12
                new PlanetModel("Jupiter", 30.0, 50.0, 0.083, "/Sources/Images/Jupiter.jpeg"), //60 240     25
                new PlanetModel("Satrun", 30.0, 44.0, 0.033, "/Sources/Images/Saturn.jpeg"), // 60 300      22
                new PlanetModel("Uranus", 20.0, 36.0, 0.012, "/Sources/Images/Uranus.jpeg"), // 40 340      18
                new PlanetModel("Neptune", 20.0, 36.0, 0.006, "/Sources/Images/Neptune.jpeg") // 40    380      18


            };
        }







    }
}
