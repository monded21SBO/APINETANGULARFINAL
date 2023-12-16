using Plagas.Dto.Request;
using Plagas.Entities;
using Plagas.Services.Interfaces;


namespace Control_Plagas.Endpoints
{
    public static class HomeEndpoints
    {
        public static void MapHomeEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/Home", async (ITrampasService trampasService, ITiposService tiposService, CancellationToken cancellationToken) =>
            {
                var trampas = await trampasService.ListAsync(new TrampasSearch()
                {
                    Page = 1,
                    Rows = 50
                }, cancellationToken);

                var tipos = await tiposService.ListAsync();

                return Results.Ok(new
                {
                    trampas = trampas.Data,
                    tipos = tipos.Data,
                    Success = true
                });
            }).WithDescription("Permite mostrar los endpoints de la pagina principal")
                .WithOpenApi();
        }
    }
}
