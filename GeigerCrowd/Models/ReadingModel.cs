using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GeigerCrowd.Models
{
    public class ReadingPoint
    {
        public int ID  {get; set; }
        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Reading, in mSv")]
        public double Reading
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Latitude")]
        public double Latitude
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Longitude")]
        public double Longitude
        {
            get;
            set;
        }

        public ReadingPoint(double reading, double latitude, double longitude)
        {
            this.Reading = reading;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Date = System.DateTime.Now;
        }

        public ReadingPoint()
        {
        }
    }

    public class MapPoint
    {

        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }

        public MapPoint(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public MapPoint()
        {
        }

    }

}
