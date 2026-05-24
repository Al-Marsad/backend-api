using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class LegalTeamMemberNote
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public string Content { get; set; }

        
        [ForeignKey(nameof(Incident))]
        public int IncidentId { get; set; }
        public virtual Incident Incident { get; set; }


        [ForeignKey(nameof(LegalTeamMember))]
        public string LegalTeamMemberId { get; set; }
        public virtual AppUser LegalTeamMember { get; set; }

    }
}
