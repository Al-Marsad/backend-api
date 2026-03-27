using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class NewsItem
    {
        public int Id { get; set; }
        
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(10000)]
        public string Body { get; set; }
        public DateTime? PublishDate { get; set; }

        [MaxLength(1000)]
        public string Summary { get; set; }
        public bool IsPublished { get; set; }

        [ForeignKey(nameof(WrittenBy))]
        public string WrittenById { get; set; }
        
        [ForeignKey(nameof(Incident))]
        public int IncidentId { get; set; }

        public virtual AppUser WrittenBy { get; set; }
        public virtual Incident Incident { get; set; }
    }
}
