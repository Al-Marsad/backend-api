using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace DAL.Entities
{
    public class Residence
    {
        public int Id { get; set; }
        public HousingType HousingType { get; set; }
        public OwnershipStatus OwnershipStatus { get; set; }
        public DateTime ResidenceStartDate { get; set; }
        public bool WasPermanentlyOccupied { get; set; }

        [ForeignKey(nameof(Incident))]
        public int IncidentId { get; set; }
        public virtual Incident Incident { get; set; }
        public virtual List<Evidence> OwnershipDocuments { get; set; } = new();

    }
}
