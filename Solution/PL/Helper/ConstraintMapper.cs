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
                var c when c.Contains("Email", StringComparison.OrdinalIgnoreCase) =>
                    new() { ["Email"] = "Email is already taken." },

                var c when c.Contains("UserName", StringComparison.OrdinalIgnoreCase) =>
                    new() { ["UserName"] = "Username is already taken." },

                var c when c.Contains("PhoneNumber", StringComparison.OrdinalIgnoreCase) =>
                    new() { ["PhoneNumber"] = "Phone number is already taken." },

                _ =>
                    new() { ["General"] = "Duplicate value violates unique constraint." }
            };
        }
    }
}
