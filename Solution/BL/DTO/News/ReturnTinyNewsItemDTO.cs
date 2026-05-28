using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.News
{
    public class ReturnTinyNewsItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsPublished { get; set; }
        public string Summary { get; set; }
        public DateTime WritingDate { get; set; }
    }
}
