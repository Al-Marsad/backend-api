using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO.Classification;

namespace BL.DTO.Question
{
    public class ReturnFullQuestionDTO
    {
        public int Id { get; set; }
        public string QuestionBody { get; set; }

        public List<ReturnIncidentClassTypeDTO> IncidentClassTypes { get; set; }
    }
}
