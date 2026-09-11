namespace AccountDashboard.Web.Common
{
    
    public class SelectedCardContext
    {
        public Guid CardId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string CardNumberMasked { get; set; } = string.Empty;
    }
}
