namespace PL.Helper
{
    public class ConstraintMapper
    {
        public static Dictionary<string, string> MapPostgresConstraint(string? constraint)
        {
            if (string.IsNullOrEmpty(constraint))
            {
                return new Dictionary<string, string>
                {
                    { "General", "Duplicate value violates unique constraint." }
                };
            }

            return constraint switch
            {
                var c when c.Contains("email", StringComparison.OrdinalIgnoreCase) =>
                    new() { ["Email"] = "Email is already taken." },

                var c when c.Contains("user_name", StringComparison.OrdinalIgnoreCase) =>
                    new() { ["UserName"] = "Username is already taken." },

                var c when c.Contains("phone_number", StringComparison.OrdinalIgnoreCase) =>
                    new() { ["PhoneNumber"] = "Phone number is already taken." },
                
                var c when c.Contains("arabic_name", StringComparison.OrdinalIgnoreCase) =>
                new() { ["ArabicName"] = "Arabic name is already taken." },
                
                var c when c.Contains("english_name", StringComparison.OrdinalIgnoreCase) =>
                new() { ["EnglishName"] = "English name is already taken." },

                var c when c.Contains("pk", StringComparison.OrdinalIgnoreCase) =>
               new() { ["Primary Key"] = "Primary key is already taken." },

                var c when c.Contains("national", StringComparison.OrdinalIgnoreCase) =>
                new() { ["National Id"] = "Primary key is already taken." },

                _ =>
                    new() { ["General"] = "Duplicate value violates unique constraint." }
            };
        }
    }
}
