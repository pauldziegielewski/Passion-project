using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passion_project.Models
{
    public class Trail
    {
        [Key]
        public int TrailID { get; set; }
        public string TrailName { get; set; }


        //A TRAIL BELONGS TO (ONE) LOCATION
        //A LOCATION CAN HAVE (MULTIPLE) TRAILS
        [ForeignKey("Location")]
        public int LocationID { get; set; }
        //VIRTUAL allows for a concept called "method overriding"
        //VIRTUAL allows for flexbility and extensibility in object oriented programming
        //VIRTUAL keyword is a special word that tells the computer that this property can be overridden or changed by other parts of the code. It allows us to add more details or behavior to this property later if we need to.
        public virtual Location Location { get; set; }


        //A (TRAIL) CAN HAVE MULTIPLE HIKING (FEATURES)
        public ICollection<Feature> Features { get; set; }
    }

    public class TrailDto
    {
        public int TrailID { get; set; }
        public string TrailName { get; set; } 

        public int LocationID { get; set; }
        public string LocationName { get; set; }

        public bool TrailHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}