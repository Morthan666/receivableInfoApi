using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReceivableInfoApi.Common.Model;
using ReceivableInfoApi.Common.Services;

namespace ReceivableInfoApi.WebApi.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class ReceivablesController : ControllerBase
{
    private readonly IReceivableCRUDService _crudService;
    private readonly ILogger<ReceivablesController> _logger;

    public ReceivablesController(
        IReceivableCRUDService crudService,
        ILogger<ReceivablesController> logger)
    {
        _crudService = crudService;
        _logger = logger;
    }

    /// <summary>
    /// Get a receivable by reference
    /// </summary>
    /// <param name="reference"></param>
    /// <returns>Receivable</returns>
    [HttpGet]
    public async Task<ActionResult<Receivable[]>> Get() => await _crudService.GetAll();

    /// <summary>
    /// Get a receivable by reference
    /// </summary>
    /// <param name="reference"></param>
    /// <returns>Receivable</returns>
    [HttpGet("/{reference}")]
    public async Task<ActionResult<Receivable>> Get([FromRoute]string reference)
    {
        _logger.LogInformation("Receivable {reference} requested", reference);
        return await _crudService.Get(reference);
    }
    
    /// <summary>
    /// Create a new receivable
    /// </summary>
    /// <param name="reference"></param>
    /// <returns>Receivable</returns>
    [HttpPost]
    public async Task<ActionResult> Post(Receivable receivable)
    { 
        var created = await _crudService.Create(receivable);
        if (created)
        {
            _logger.LogInformation("Receivable {reference} created", receivable.Reference);
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
            
        _logger.LogInformation("Receivable {reference} updated", receivable.Reference);
        return Ok("Receivable updated");
    }
    
    /// <summary>
    /// Update a receivable
    /// </summary>
    /// <param name="reference"></param>
    [HttpPut("/{reference}")]
    public async Task<ActionResult> Put(Receivable receivable)
    {
        await _crudService.Update(receivable);
        _logger.LogInformation("Receivable {reference} updated", receivable.Reference);
        return Ok("Receivable updated");
    }

    /// <summary>
    /// Delete a receivable by reference
    /// </summary>
    /// <param name="reference"></param>
    [HttpDelete("/{reference}")]
    public async Task<ActionResult> Delete(string reference)
    {
        await _crudService.Delete(reference);
        _logger.LogInformation("Receivable {reference} deleted", reference);
        return Ok("Receivable deleted");
    }
}