using BL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.Exceptions;

namespace BL.Services
{
    public class VictimService : IVictimService
    {
        private readonly IVictimRepository _victimRepository;
        public VictimService(IVictimRepository victimRepository)
        {
            this._victimRepository = victimRepository;
        }
        public async Task<bool> VictimExists(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                throw new ValidationException("Validation faild",
                    new {NationalId = "It can't be null or empty"});

            var victim = await _victimRepository.GetByNationalIdAsync(nationalId);
            return victim != null;
        }
    }
}