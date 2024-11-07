namespace Pastebin_api.Services
{
    public class JWTOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
    }
}
