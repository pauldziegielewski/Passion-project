using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passion_project.Models.ViewModels
{
    public class UpdateTrail
    {
        //This viewmodel is a class which stores information that we need to present to /Trail/Update/{id}

        // Existing trails
        public TrailDto SelectedTrail { get; set; }

        // All locations to choose from when updating trail
        public IEnumerable<LocationDto> LocationOptions { get; set; }

        // FEATURES TO BE INCLUDED???

    }
}