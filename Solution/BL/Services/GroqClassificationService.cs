using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BL.Models;
using BL.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BL.Services
{
    public class GroqClassificationService : IClassificationService<IncidentClassificationInput, MatchedArticle, ClassificationResult> 
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private const string Model = "llama-3.3-70b-versatile";

        public string ModelName => Model;   

        public GroqClassificationService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Groq:ApiKey"] ??
                throw new InvalidOperationException("Groq ApiKey is not configured. add it to appsettings.json");

        }

        public async Task<ClassificationResult> ClassifyAsync(
            IncidentClassificationInput input,
            List<MatchedArticle> matchedArticles)
        {
            var context = BuildContext(matchedArticles);
            var fullInput = input.BuildFullContext();

            var systemPrompt = """
                أنت محلل قانوني متخصص في القانون الجنائي الدولي ونظام روما الأساسي.
                مهمتك تحليل الحوادث بدقة وحياد تام وفق معايير الإثبات القانوني الدولي.

                ستتلقى:
                - معطيات الحادثة: وصف تفصيلي، وصف الجاني، بيانات استبيان، وشهادات الضحايا.
                - قائمة مواد مقترحة من نظام روما الأساسي كمرجع فقط.

                ══════════════════════════════════════════
                قواعد صارمة يجب الالتزام بها:
                ══════════════════════════════════════════

                1. المواد المقترحة هي اقتراحات للمراجعة فقط، وليست انتهاكات مؤكدة.
                   لا تعتبر أي مادة منتهكة لمجرد ورودها في القائمة.

                2. لا تُدرج مادة في التحليل إلا إذا توافرت جميع عناصرها القانونية معاً
                   في معطيات الحادثة المقدمة، وليس بعضها فقط.

                3. إذا كانت المعطيات غامضة أو غير كافية لإثبات انتهاك مادة معينة،
                   استبعدها تماماً ولا تُدرجها.

                4. لا تستنتج نوايا أو وقائع غير مذكورة صراحةً في معطيات الحادثة.
                   التزم بما هو موثق فقط.

                5. الأدلة اللازمة لإثبات الانتهاك يجب أن تكون واضحة في:
                   وصف الحادثة، أو وصف الجاني، أو بيانات الاستبيان، أو شهادات الضحايا.

                6. إذا لم تنتهك الحادثة أي مادة بشكل قابل للإثبات، بكل بساطة تجاهله تماما.

                ══════════════════════════════════════════
                شكل الإجابة المطلوب — فقرتان فقط:
                ══════════════════════════════════════════

                الفقرة الأولى بعنوان "القواعد المنتهكة":
                اذكر فقط المواد و الجرائم بضبط التي تتوفر جميع عناصرها القانونية في معطيات الحادثة،
                مع ذكر رقم المادة واسم الجريمة، إن لم تتوفر أي انتهاكات قابلة للإثبات، صرّح بذلك.

                الفقرة الثانية بعنوان "التعليل القانوني":
                اشرح لكل مادة أدرجتها: ما هي عناصر الجريمة المنصوص عليها، وكيف تتطابق
                تحديداً مع الوقائع الموثقة في معطيات الحادثة. وما هو الدليل المباشر من المعطيات على كل انتهاك وجريمة

                لا تكتب أي شيء خارج هاتين الفقرتين.
             """;

            var userPrompt = $"""
                ## المواد القانونية المرجعية من نظام روما الأساسي:
                {context}

                ## معطيات الحادثة:
                {fullInput}
             """;

            var requestBody = new
            {
                model = Model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user",   content = userPrompt   }
                },
                temperature = 0.2
            };

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _http.PostAsJsonAsync(
                "https://api.groq.com/openai/v1/chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Groq classification failed ({response.StatusCode}): {error}");
            }

            var groqResponse = await response.Content.ReadFromJsonAsync<GroqResponse>()
                ?? throw new InvalidOperationException("Empty response from Groq.");

            var analysis = groqResponse.Choices[0].Message.Content.Trim();

            return new ClassificationResult
            {
                Analysis = analysis
            };
        }

        private static string BuildContext(List<MatchedArticle> articles) =>
            string.Join("\n---\n", articles.Select((a, i) =>
                $"[{i + 1}] الفئة: {a.Category}\n" +
                $"    المادة: {a.ArticleNumber}\n" +
                $"    المصدر كامل: {a.FullReference}\n" +
                $"    عنوان الجريمة: {a.Title}\n" +
                $"    النص القانوني: {a.LegalText}\n" +
                $"    العناصر: {a.ElementsOfCrime}\n" +
                $"    المتطلبات السياقية: {a.ContextualRequirement}\n" +
                $"    أمثلة السلوك: {a.ConductExamples}\n" +
                $"    درجة التشابه: {a.Score:P0}"));
    }

    file record GroqResponse(
        [property: JsonPropertyName("choices")] List<GroqChoice> Choices);

    file record GroqChoice(
        [property: JsonPropertyName("message")] GroqMessage Message);

    file record GroqMessage(
        [property: JsonPropertyName("content")] string Content);
}
