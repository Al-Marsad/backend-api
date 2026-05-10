using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Enums
{
    public enum InjuryStatus
    {
        Other,
        Killed,
        Injured,
        Arrested,
        Displaced
    }
}
