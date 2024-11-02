namespace Pastebin_api.Services
{
    public class HashGenerator
    {
        public string GenerateUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
