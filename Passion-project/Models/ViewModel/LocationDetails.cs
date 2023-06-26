using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Passion_project.Models.ViewModels
{
    public class LocationDetails
    {
        public LocationDto SelectedLocation { get; set; }
        public IEnumerable<TrailDto> RelatedTrails { get; set; }
    }


}