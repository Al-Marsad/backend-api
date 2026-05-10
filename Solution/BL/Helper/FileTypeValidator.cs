using DAL.Enums;

namespace BL.Helper
{
    public class FileTypeValidator
    {
        public static bool IsMatchingType(string ContentType, EvidenceType type)
        {
            return type switch
            {
                EvidenceType.Image => ContentType.Contains("image"),
                EvidenceType.Video => ContentType.Contains("video"),
                _ => true
            };
        }
    }
}
