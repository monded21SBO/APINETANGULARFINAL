
using Microsoft.AspNetCore.Mvc;
using Plagas.Dto.Request;
using Plagas.Services.Interfaces;

namespace Control_Plagas.Controllers;

    [ApiController]
[Route("api/[controller]")]


    public class TrampasController : ControllerBase
    {
    
    private readonly ITrampasService _service;

    public TrampasController(ITrampasService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TrampasSearch search, CancellationToken cancellationToken = default)
    {
        var response = await _service.ListAsync(search, cancellationToken);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await _service.FindByIdAsync(id);

        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TrampasDtoRequest request)
    {
        var response = await _service.AddAsync(request);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] TrampasDtoRequest request)
    {
        var response = await _service.UpdateAsync(id, request);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);
        return Ok(response);
    }

  
    



}

