using System.Net;

namespace AccountDashboard.Web.Common
{
    /// <summary>
    /// Wrapper de resultado para toda llamada a CardAPI.
    /// Permite que los controladores de MVC muestren mensajes de error claros al usuario
    /// sin propagar excepciones ni detalles tecnicos del backend hacia la vista.
    /// </summary>
    public class ApiResult
    {
        public bool Success { get; init; }
        public HttpStatusCode? StatusCode { get; init; }
        public string? ErrorMessage { get; init; }

        public static ApiResult Ok() => new() { Success = true };

        public static ApiResult Fail(string message, HttpStatusCode? statusCode = null) =>
            new() { Success = false, ErrorMessage = message, StatusCode = statusCode };
    }

    /// <summary>
    /// Version generica de <see cref="ApiResult"/> que ademas transporta el dato deserializado
    /// cuando la llamada fue exitosa.
    /// </summary>
    /// <typeparam name="T">Tipo del contrato (DTO) devuelto por CardAPI.</typeparam>
    public class ApiResult<T> : ApiResult
    {
        public T? Data { get; init; }

        public static ApiResult<T> Ok(T data) => new() { Success = true, Data = data };

        public new static ApiResult<T> Fail(string message, HttpStatusCode? statusCode = null) =>
            new() { Success = false, ErrorMessage = message, StatusCode = statusCode };
    }
}
