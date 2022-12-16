namespace appBD.Models
{
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string? email { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public DateTime? date_of_birth { get; set; }
    }
}
