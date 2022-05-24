namespace Screechr.Model
{
    public class Token
    {
        public string? AccessToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public string? TokenType { get; set; }
    }
}
