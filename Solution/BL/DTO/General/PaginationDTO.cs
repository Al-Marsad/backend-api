using System.ComponentModel.DataAnnotations;

namespace BL.DTO.General
{
    public class PaginationDTO
    {

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1.")]
        public int Page { get; set; } = 1;

        [Range(0, 50, ErrorMessage = "PageSize must be between 0 and 50.")]
        public int PageSize { get; set; } = 20;
    }
}
