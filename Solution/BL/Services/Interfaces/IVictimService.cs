using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Interfaces
{
    public interface IVictimService
    {
        Task<bool> VictimExists(string nationalId);
    }
}
