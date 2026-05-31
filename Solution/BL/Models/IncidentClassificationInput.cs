

namespace BL.Models
{
    public class IncidentClassificationInput
    {
        public string? DetailedDescription { get; set; }
        public string? PerpetratorDescription { get; set; }
        public string? QuestionnaireJSON { get; set; }
        public List<string> PersonalNarratives { get; set; } = new();   

        public string BuildFullContext()
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(DetailedDescription))
                parts.Add($"وصف الحادثة:\n{DetailedDescription}");

            if (!string.IsNullOrWhiteSpace(PerpetratorDescription))
                parts.Add($"وصف الجاني:\n{PerpetratorDescription}");

            if (!string.IsNullOrWhiteSpace(QuestionnaireJSON))
                parts.Add($"بيانات الاستبيان:\n{QuestionnaireJSON}");

            if (PersonalNarratives.Count > 0)
            {
                var narratives = string.Join("\n\n",
                    PersonalNarratives
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .Select((n, i) => $"شهادة الضحية [{i + 1}]:\n{n}"));

                if (!string.IsNullOrWhiteSpace(narratives))
                    parts.Add(narratives);
            }

            return string.Join("\n\n─────────────────\n\n", parts);
        }
    }
}
