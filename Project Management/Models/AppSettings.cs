namespace Project_Management.Models
{
    public class AppSettings
    {
        public string ConnStr { get; set; }
        public JwtSettings Jwt { get; set; }
    }

    public class JwtSettings
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }

}
