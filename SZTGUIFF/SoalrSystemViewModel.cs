using CommunityToolkit.Mvvm.ComponentModel;
using SolarSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SolarSystem.ViewModels
{
    public partial class SolarSystemViewModel : ObservableObject
    {
        [ObservableProperty]
        public DateTime selectedDate = new DateTime(2492, 05, 06);

        public string SelectedPlanet()
        { 
            
        }


            



    }
}
