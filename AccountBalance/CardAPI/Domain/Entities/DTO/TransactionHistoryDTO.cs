namespace CardAPI.Domain.Entities.DTO
{
    public class TransactionHistoryDTO
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public int State { get; set; }
    }

    public class TransactionHistoryResponseDTO
    {
        public Guid CardId { get; set; }
        public List<TransactionHistoryDTO> Transactions { get; set; }
    }

    public class TransactionHistoryRequestDTO
    {
        public Guid CardId { get; set; }
        public string UserId { get; set; }
    }
}
