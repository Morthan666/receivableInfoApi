using Microsoft.AspNetCore.Mvc;
using ReceivableInfoApi.Common.Model;
using ReceivableInfoApi.Common.Services;

namespace ReceivableInfoApi.WebApi.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class ReceivablesController : Controller
{
    private readonly IReceivableCRUDService _crudService;
    private readonly IReceivableStatisticsService _statisticsService;
    private readonly ILogger<ReceivablesController> _logger;

    public ReceivablesController(
        IReceivableCRUDService crudService,
        IReceivableStatisticsService statisticsService,
        ILogger<ReceivablesController> logger)
    {
        _crudService = crudService;
        _statisticsService = statisticsService;
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
    [HttpGet("{reference}")]
    public async Task<ActionResult<Receivable>> Get([FromRoute] string reference)
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
            return new CreatedResult(receivable.Reference, receivable);
        }

        _logger.LogInformation("Receivable {reference} exists", receivable.Reference);
        return this.SeeOther(receivable.Reference);
    }

    /// <summary>
    /// Update a receivable
    /// </summary>
    /// <param name="reference"></param>
    [HttpPut("{reference}")]
    public async Task<ActionResult> Put(Receivable receivable)
    {
        var updated = await _crudService.Update(receivable);
        if (updated)
        {
            _logger.LogInformation("Receivable {reference} updated", receivable.Reference);
            return Ok("Receivable updated");
        }

        _logger.LogInformation("Receivable {reference} created", receivable.Reference);
        return new CreatedResult(receivable.Reference, receivable);
    }

    /// <summary>
    /// Delete a receivable by reference
    /// </summary>
    /// <param name="reference"></param>
    [HttpDelete("{reference}")]
    public async Task<ActionResult> Delete(string reference)
    {
        await _crudService.Delete(reference);
        _logger.LogInformation("Receivable {reference} deleted", reference);
        return Ok("Receivable deleted");
    }

    /// <summary>
    /// Get the summary of open invoices value
    /// </summary>
    [HttpGet("openValueSummary")]
    public async Task<ActionResult<decimal>> GetOpenValueSummary()
    {
        return await _statisticsService.GetOpenValueSummary();
    }

    /// <summary>
    /// Get the summary of closed invoices value
    /// </summary>
    [HttpGet("closedValueSummary")]
    public async Task<ActionResult<decimal>> GetClosedValueSummary() => await _statisticsService.GetClosedValueSummary();
}