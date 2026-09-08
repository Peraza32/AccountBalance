namespace CardAPI.Domain.Entities.DTO
{
    public class LogDTO
    {
        public string origin { get; set; } = "CardAPI generic";
        public string description { get; set; } = "CardAPI generic log";
        public DateTime logDate { get; set; } = DateTime.Now;
    }
}
