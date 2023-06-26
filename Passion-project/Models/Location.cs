using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Passion_project.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }


    public class LocationDto
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }
}