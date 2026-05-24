
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities;
using DAL.Enums;

namespace BL.DTO.Activity
{
    public class ReturnActivityDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public ActivityType Type { get; set; }
        public string MadeById { get; set; }
    }
}
