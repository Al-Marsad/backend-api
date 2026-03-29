namespace PL.Helper
{
    public class GeneralResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic? Data { get; set; }
    }
}
