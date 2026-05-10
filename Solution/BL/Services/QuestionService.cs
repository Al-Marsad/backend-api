using AutoMapper;
using BL.DTO.Question;
using BL.Services.Interfaces;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class QuestionService : IQuestionService
    {   
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        public QuestionService(IQuestionRepository questionRepository,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }
        public async Task<List<ReturnFullQuestionDTO>> GetQuestionByIncidentClassTypeAsync(int[] ids)
        {
            if(ids == null || ids.Length == 0)
            {
                throw new ValidationException("Ids array cannot be null or empty");
            }

            var questions = await _questionRepository.GetFullQuestionByIncidentClassTypeAsync(ids);
            
            return _mapper.Map<List<ReturnFullQuestionDTO>>(questions); 
        }

    }
}
