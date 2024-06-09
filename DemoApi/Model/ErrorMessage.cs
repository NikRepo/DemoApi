namespace DemoApi.Model
{
    /// <summary>
    /// This is error response - to keep consistent throughout application
    /// </summary>
    public class ApiErrorMessage
    {
        public string? Error { get; set; }
        public string? Details { get; set; }
    }
}
