using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passion_project.Models.ViewModels
{
    public class TrailDetails
    {

        public TrailDto SelectedTrail { get; set; }
        public IEnumerable<FeatureDto> AvailableFeatures { get; set; }
        public IEnumerable<FeatureDto> AFeatures { get; set; }
        public bool IsAdmin { get; set; }

    }
}