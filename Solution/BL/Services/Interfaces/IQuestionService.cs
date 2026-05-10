using BL.DTO.Question;

namespace BL.Services.Interfaces
{
    public interface IQuestionService
    {
        public Task<List<ReturnFullQuestionDTO>> GetQuestionByIncidentClassTypeAsync(int[] ids);

    }
}
