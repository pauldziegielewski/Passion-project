using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Passion_project.Models
{
    
    public class Feature
    {
        [Key]
        public int FeatureID { get; set; }
        public string FeatureName { get; set; }

        //A (FEATURE) CAN BELONG TO MULTIPLE HIKING TRAILS
        public ICollection <Trail> Trails { get; set; }
    }

    public class FeatureDto
    {
        public int FeatureID { get; set; }
        public string FeatureName { get; set; }
    }


}
