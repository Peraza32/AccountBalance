using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AccountDashboard.Web.Common;
using Microsoft.Extensions.Logging;

namespace AccountDashboard.Web.Services
{
    /// <summary>
    /// Logica comun para consumir CardAPI vía HttpClient con manejo de errores consistente.
    /// Toda la responsabilidad de "API consumption" + "Error handling" del frontend vive aqui,
    /// para no repetirla en cada servicio ni en cada controlador.
    /// </summary>
    public abstract class ApiServiceBase
    {
        protected readonly HttpClient HttpClient;
        protected readonly ILogger Logger;

        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        protected ApiServiceBase(HttpClient httpClient, ILogger logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }

        protected async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string requestUri, TRequest payload)
        {
            try
            {
                using var response = await HttpClient.PostAsJsonAsync(requestUri, payload, JsonOptions);
                return await ReadResponseAsync<TResponse>(response, requestUri);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return HandleTransportException<TResponse>(ex, requestUri);
            }
        }

        protected async Task<ApiResult> PostAsync<TRequest>(string requestUri, TRequest payload)
        {
            try
            {
                using var response = await HttpClient.PostAsJsonAsync(requestUri, payload, JsonOptions);
                return await ReadResponseAsync(response, requestUri);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return HandleTransportException(ex, requestUri);
            }
        }

        protected async Task<ApiResult<TResponse>> GetAsync<TResponse>(string requestUri)
        {
            try
            {
                using var response = await HttpClient.GetAsync(requestUri);
                return await ReadResponseAsync<TResponse>(response, requestUri);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return HandleTransportException<TResponse>(ex, requestUri);
            }
        }

        private async Task<ApiResult<TResponse>> ReadResponseAsync<TResponse>(HttpResponseMessage response, string requestUri)
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return ApiResult<TResponse>.Ok(default!);
                }

                try
                {
                    var data = await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
                    return ApiResult<TResponse>.Ok(data!);
                }
                catch (JsonException ex)
                {
                    Logger.LogError(ex, "Respuesta invalida de CardAPI en {RequestUri}", requestUri);
                    return ApiResult<TResponse>.Fail("La respuesta del servidor no tiene el formato esperado.", response.StatusCode);
                }
            }

            return ApiResult<TResponse>.Fail(await BuildErrorMessageAsync(response, requestUri), response.StatusCode);
        }

        private async Task<ApiResult> ReadResponseAsync(HttpResponseMessage response, string requestUri)
        {
            if (response.IsSuccessStatusCode)
            {
                return ApiResult.Ok();
            }

            return ApiResult.Fail(await BuildErrorMessageAsync(response, requestUri), response.StatusCode);
        }

        private async Task<string> BuildErrorMessageAsync(HttpResponseMessage response, string requestUri)
        {
            var body = await SafeReadBodyAsync(response);
            Logger.LogWarning(
                "CardAPI respondio {StatusCode} para {RequestUri}. Cuerpo: {Body}",
                (int)response.StatusCode, requestUri, body);

            return response.StatusCode switch
            {
                HttpStatusCode.BadRequest => string.IsNullOrWhiteSpace(body)
                    ? "Los datos enviados no son validos."
                    : CleanBody(body),
                HttpStatusCode.NotFound => "No se encontro la informacion solicitada.",
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => "No tiene permisos para realizar esta accion.",
                HttpStatusCode.InternalServerError => "Ocurrio un error en el servidor. Intente nuevamente en unos minutos.",
                _ => "Ocurrio un error inesperado al comunicarse con el servidor."
            };
        }

        private static async Task<string> SafeReadBodyAsync(HttpResponseMessage response)
        {
            try
            {
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string CleanBody(string body)
        {
            
            var trimmed = body.Trim().Trim('"');
            return trimmed.Length > 300 ? "Los datos enviados no son validos." : trimmed;
        }

        private ApiResult<TResponse> HandleTransportException<TResponse>(Exception ex, string requestUri)
        {
            Logger.LogError(ex, "Fallo de comunicacion con CardAPI en {RequestUri}", requestUri);
            var message = ex is TaskCanceledException
                ? "El servidor tardo demasiado en responder. Intente nuevamente."
                : "No fue posible conectarse con el servidor. Verifique su conexion e intente nuevamente.";
            return ApiResult<TResponse>.Fail(message);
        }

        private ApiResult HandleTransportException(Exception ex, string requestUri)
        {
            Logger.LogError(ex, "Fallo de comunicacion con CardAPI en {RequestUri}", requestUri);
            var message = ex is TaskCanceledException
                ? "El servidor tardo demasiado en responder. Intente nuevamente."
                : "No fue posible conectarse con el servidor. Verifique su conexion e intente nuevamente.";
            return ApiResult.Fail(message);
        }
    }
}
