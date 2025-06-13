namespace JWT.Helpers
{
    public class JWThelper
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpirationMinutes { get; set; }
    }
}
