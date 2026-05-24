using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.LegalNote
{
    public class ReturnLegalNoteDTO
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
    }
}
