namespace AccountDashboard.Web.Common
{
    /// <summary>
    /// El Id (Guid) de una tarjeta es un dato sensible: nunca debe mostrarse en pantalla,
    /// viajar en la URL (querystring) ni quedar expuesto en logs de la aplicacion.
    /// Esta clase centraliza esas reglas para que ningun controlador/vista las reimplemente.
    /// </summary>
    public static class CardMasking
    {
        /// <summary>
        /// Devuelve una version segura para logging de un Guid de tarjeta (solo los ultimos 4 caracteres).
        /// Nunca registrar el Guid completo en logs de la aplicacion frontend.
        /// </summary>
        public static string ForLog(Guid cardId)
        {
            var value = cardId.ToString("N");
            return value.Length >= 4 ? $"****{value[^4..]}" : "****";
        }

       
        public static string FormatCardNumber(string? cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return "****";
            }

            var digitsOnly = cardNumber.Trim();
            var lastFour = digitsOnly.Length > 4 ? digitsOnly[^4..] : digitsOnly;
            return $"**** **** **** {lastFour}";
        }
    }
}
