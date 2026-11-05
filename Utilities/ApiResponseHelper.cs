using System.Net;

namespace clientside_ticketmanutenzione.Utilities;

public static class ApiResponseHelper
{
    public static async Task<IResult> HandleError(HttpResponseMessage response)
    {
        var error = await response.Content
            .ReadFromJsonAsync<Dictionary<string,string>>();
        string message = error?.GetValueOrDefault("message") ?? "Errore durante la comunicazione con il server";
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
                    "Endpoint non trovato",
                    statusCode: StatusCodes.Status502BadGateway),

            StatusCodes.Status429TooManyRequests =>
                Results.Problem(
                    "Troppi tentativi. Riprova più tardi.",
                    statusCode: StatusCodes.Status429TooManyRequests),

            _ =>
                Results.Problem(
                    message,
                    statusCode: StatusCodes.Status502BadGateway)
        };
    }
}
