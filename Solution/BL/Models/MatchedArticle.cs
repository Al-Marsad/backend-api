namespace BL.Models
{
    public class MatchedArticle
    {
        public string Category { get; set; } = "";
        public string ArticleNumber { get; set; } = "";
        public string FullReference { get; set; } = "";
        public string Title { get; set; } = "";
        public string LegalText { get; set; } = "";
        public string ElementsOfCrime { get; set; } = "";
        public string ContextualRequirement { get; set; } = "";
        public string ConductExamples { get; set; } = "";
        public string TextToVectorize { get; set; } = "";
        public float Score { get; set; }
    }
}
