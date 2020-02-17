using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watermango.Models 
{

    public enum PlantStatus {
        Idle = 1,
        Watering,
        Resting
    }

    public class Plant 
    {
        public long Id { get; set;}
        public string Name { get; set; }
        public string Location { get; set; }
        public PlantStatus State { get; set; }
        public DateTime LastUpdated { get; set; }
        public String PreviewUrl { get; set; }

        public Plant()
        {
            this.State = PlantStatus.Idle;
            this.LastUpdated = DateTime.Now;
        }
    }

}