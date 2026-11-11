using System.Text.Json;

namespace clientside_ticketmanutenzione.Utilities;

public static class ApiResponseHelper
{
    public static async Task<IResult> HandleError(HttpResponseMessage response)
    {
        using var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var root = document?.RootElement;
        string message = "Errore durante la comunicazione con il server";
        if (root.HasValue && root.Value.ValueKind == JsonValueKind.Object)
        {
            //sostituire con un for each che destrutturi e ristrutturi il dizionario ricevuto
            if (root.Value.TryGetProperty("message", out var property)
                || root.Value.TryGetProperty("detail", out property)
                || root.Value.TryGetProperty("title", out property))
            {
                message = property.ValueKind == JsonValueKind.String
                    ? property.GetString()!
                    : property.ToString();
            }
        }
        //string message = await ExtractMessage(response);

        //var error = await response.Content
        //    .ReadFromJsonAsync<Dictionary<string,string>>();
        //string message = error?.GetValueOrDefault("message") ?? "Errore durante la comunicazione con il server";
        return (int)response.StatusCode switch
        {
            StatusCodes.Status400BadRequest =>
                Results.BadRequest(new
                {
                    message
                }),

            StatusCodes.Status401Unauthorized =>
                Results.Json(
                    new { message },
                    statusCode: StatusCodes.Status401Unauthorized),

            StatusCodes.Status403Forbidden =>
                Results.Json(
                    new { message },
                    statusCode: StatusCodes.Status403Forbidden),

            StatusCodes.Status404NotFound =>
                Results.Problem(
                    message,
                    statusCode: StatusCodes.Status502BadGateway),

            StatusCodes.Status429TooManyRequests =>
                Results.Problem(
                    message,
                    statusCode: StatusCodes.Status429TooManyRequests),
            
            StatusCodes.Status500InternalServerError =>
                Results.Problem(
                    message,
                    statusCode: StatusCodes.Status500InternalServerError),
            
            _ =>
                Results.Problem(
                    message,
                    statusCode: StatusCodes.Status502BadGateway)
        };
    }
}
