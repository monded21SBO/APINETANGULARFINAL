using Plagas.Services.Interfaces;


namespace Control_Plagas.Endpoints
{
    public static class ReportEndpoints
    {
        public static void MapReports(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("api/Reports")
                .WithDescription("Reportes de Plagas")
                .WithTags("Reports");

            group.MapGet("/", async (IVisitaService service, string dateStart, string dateEnd) =>
            {
                var response = await service.GetReportSaleAsync(DateTime.Parse(dateStart), DateTime.Parse(dateEnd));

                return response.Success ? Results.Ok(response) : Results.BadRequest(response);
            });
        }
    }
}
