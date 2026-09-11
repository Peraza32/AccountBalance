namespace AccountDashboard.Web.Common
{
    /// <summary>
    /// Configuracion tipada para el consumo de CardAPI (backend).
    /// Se enlaza desde appsettings.json -> seccion "ApiSettings".
    /// </summary>
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
    }
}
