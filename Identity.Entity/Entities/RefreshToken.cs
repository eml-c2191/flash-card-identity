namespace Identity.Entity.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string RefreshTokenHash { get; set; } = string.Empty;

        public DateTime ExpiredTime { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Payload { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
