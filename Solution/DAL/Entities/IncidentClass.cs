using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class IncidentClass
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        
        [ForeignKey(nameof(IncidentClassType))]
        public int IncidentClassTypeId { get; set; }   
        public IncidentClassType IncidentClassType { get; set; }
    }
}
