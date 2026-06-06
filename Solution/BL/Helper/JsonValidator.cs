using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public static class JsonValidator <T>
    {
        private static string ExtractJson(string input)
        {
            int firstBrace = input.IndexOf('{');
            int lastBrace = input.LastIndexOf('}');
            if (firstBrace >= 0 && lastBrace > firstBrace)
            {
                return input.Substring(firstBrace, lastBrace - firstBrace + 1);
            }
            throw new FormatException("Input does not contain valid JSON.");
        }   
        public static bool IsValidJson(ref string json)
        {
            try
            {
                json = ExtractJson(json);
                var obj = System.Text.Json.JsonSerializer.Deserialize<T>(json);
                return true;
            }
            catch (System.Text.Json.JsonException)
            {
                return false;
            }
        }
    }
}
