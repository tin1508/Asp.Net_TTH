using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Data;

namespace StationeryStore.Mvc.Configuration;

public static class EndpointConfig
{
    public static IEndpointRouteBuilder MapStationeryApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/stationeries/{id:int}", async (int id, AppDbContext db, HttpContext http) =>
        {
            var stationery = await db.Stationeries.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (stationery == null)
            {
                return Results.Problem(
                    type: "https://example.com/problems/stationery-not-found",
                    title: "Stationery not found",
                    detail: $"The Stationery with id {id} was not found.",
                    statusCode: StatusCodes.Status404NotFound,
                    instance: http.Request.Path);
            }
            return Results.Ok(stationery);
        });

        return endpoints;
    }
}