using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace vega.Core.Models
{
    public class Feature
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        public Feature()
        {
            this.Vehicles = new Collection<Vehicle>();
        }
    }
}