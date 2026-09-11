using System.ComponentModel.DataAnnotations;

namespace AccountDashboard.Web.Models
{
   
    public class SelectCardViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public List<ClientCardOptionViewModel> Cards { get; set; } = new();

        [Required(ErrorMessage = "Seleccione una tarjeta.")]
        [Display(Name = "Tarjeta")]
        public string SelectedCardId { get; set; } = string.Empty;
    }

    public class ClientCardOptionViewModel
    {
       
        public string CardId { get; set; } = string.Empty;

        public string CardNumberMasked { get; set; } = string.Empty;
    }
}
