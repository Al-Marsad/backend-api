using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BL.Helper
{
    public class IdentityHandler
    {
        public static void HandleIdentityErrors(IdentityResult result)
        {
            var identityErrors = result.Errors.ToList();

            var fields = identityErrors
                .GroupBy(e => FieldMapper.MapField(e.Code))
                .ToDictionary(
                    g => g.Key,
                    g => g.First().Description
                );

            bool isDuplicate = identityErrors.Any(e =>
                e.Code == "DuplicateEmail" ||
                e.Code == "DuplicateUserName"
            );

            if (isDuplicate)
                throw new ConflictException("Duplicate resource", fields);

            throw new ValidationException("Validation failed", fields);
        }
    }
}

