using Microsoft.AspNetCore.Mvc;
using Plagas.Dto.Request;
using Plagas.Services.Interfaces;



namespace Control_Plagas.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class VisitaController : ControllerBase
{

        private readonly IVisitaService _service;
        private readonly ILogger<VisitaController> _logger;

        public VisitaController(IVisitaService service, ILogger<VisitaController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VisitaDtoRequest request)
        {
            _logger.LogInformation("Id del requester es: {ConnectionId}", HttpContext.Connection.Id);

            var response = await _service.AddAsync(request.Email, request);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _service.FindByIdAsync(id);

            return response.Success ? Ok(response) : NotFound(response);
        }


    [HttpGet("ListVisitByDate")]
    public async Task<IActionResult> GetListVisitasByDate([FromQuery] VisitaByDateSearch search)
    {
        var response = await _service.ListAsync(search);

        return Ok(response);
    }

    [HttpGet("ListVisitas")]
    public async Task<IActionResult> GetListVisitas(string email, [FromQuery] VisitaByTitleSearch search)
    {
        var response = await _service.ListAsync(email, search);

        return Ok(response);
    }



}

