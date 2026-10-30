using System.Net;

namespace clientside_ticketmanutenzione.Utilities;

public static class ApiResponseHelper
{
    public static IResult HandleError(HttpStatusCode statusCode)
    {
        return (int)statusCode switch
        {
            StatusCodes.Status400BadRequest =>
                Results.BadRequest(new
                {
                    message = "Richiesta non valida."
                }),

            StatusCodes.Status401Unauthorized =>
                Results.Unauthorized(),

            StatusCodes.Status403Forbidden =>
                Results.Forbid(),

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
                    "Errore durante la comunicazione con il server.",
                    statusCode: StatusCodes.Status502BadGateway)
        };
    }
}
