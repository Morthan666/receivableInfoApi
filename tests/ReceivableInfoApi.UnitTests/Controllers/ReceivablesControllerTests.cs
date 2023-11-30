using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReceivableInfoApi.Common.Model;
using ReceivableInfoApi.Common.Services;
using ReceivableInfoApi.WebApi.Controllers;
using Shouldly;

namespace ReceivableInfoApi.UnitTests.Controllers;

public class ReceivablesControllerTests
{
    private readonly Fixture _fixture;
    private readonly ReceivablesController _controller;
    private readonly Mock<IReceivableCRUDService> _crudService;
    private readonly Mock<IReceivableStatisticsService> _statisticsService;

    public ReceivablesControllerTests()
    {
        _fixture = new Fixture();

        var logger = new Mock<ILogger<ReceivablesController>>();
        _crudService = new Mock<IReceivableCRUDService>();
        _statisticsService = new Mock<IReceivableStatisticsService>();
        _controller = new ReceivablesController(_crudService.Object, _statisticsService.Object, logger.Object);
    }

    [Fact]
    public async Task ReceivableGetAll_ReturnsAllReceivables()
    {
        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        _crudService
            .Setup(s
                => s.GetAll())
            .ReturnsAsync(receivables);
        
        var result = await _controller.Get();
        
        result.ShouldNotBeNull();
        result.Value.ShouldBeEquivalentTo(receivables);
    }
    
    [Fact]
    public async Task ReceivableGet_ReturnsRequestedReceivable()
    {
        var receivable = _fixture.Create<Receivable>();
        _crudService
            .Setup(s
                => s.Get(receivable.Reference))
            .ReturnsAsync(receivable);
        
        var result = await _controller.Get(receivable.Reference);
        
        result.ShouldNotBeNull();
        result.Value.ShouldBeEquivalentTo(receivable);
    }
    
    [Fact]
    public async Task ReceivablePost_InvokesNewReceivableCreation()
    {
        var receivable = _fixture.Create<Receivable>();
        _crudService
            .Setup(s
                => s.Create(receivable))
            .ReturnsAsync(true);
        
        var result = await _controller.Post(receivable);
        
        ((CreatedResult)result).Location.ShouldBeEquivalentTo(receivable.Reference);
        _crudService.Verify(s => s.Create(receivable), Times.Once);
    }
    
    [Fact]
    public async Task ReceivablePut_InvokesReceivableUpdate()
    {
        var receivable = _fixture.Create<Receivable>();
        _crudService
            .Setup(s
                => s.Update(receivable))
            .ReturnsAsync(true);
        
        var result = await _controller.Put(receivable.Reference, receivable);
        
        ((OkObjectResult)result).Value.ShouldBe("Receivable updated");
        _crudService.Verify(s => s.Update(receivable), Times.Once);
    }
    
    [Fact]
    public async Task ReceivablePutNotExisting_ReturnsCreatedResponse()
    {
        var receivable = _fixture.Create<Receivable>();
        _crudService
            .Setup(s
                => s.Update(receivable))
            .ReturnsAsync(false);
        
        var result = await _controller.Put(receivable.Reference, receivable);
        
        ((CreatedResult)result).Location.ShouldBeEquivalentTo(receivable.Reference);
        _crudService.Verify(s => s.Update(receivable), Times.Once);
    }
    
    [Fact]
    public async Task ReceivablePutReferenceNotMatched_ReturnsBadRequest()
    {
        var receivable = _fixture.Create<Receivable>();

        var result = await _controller.Put(_fixture.Create<string>(), receivable);
        
        ((BadRequestObjectResult)result).Value.ShouldBe("Receivable reference cannot be modified");
    }
    
    [Fact]
    public async Task ReceivableDelete_InvokesDeletion()
    {
        var receivable = _fixture.Create<Receivable>();
        _crudService
            .Setup(s
                => s.Delete(receivable.Reference));

        var result = await _controller.Delete(receivable.Reference);
        
        ((OkObjectResult)result).Value.ShouldBe("Receivable deleted");
        _crudService.Verify(s => s.Delete(receivable.Reference), Times.Once);
    }
    
    [Fact]
    public async Task OpenValueSummary_ReturnsSummary()
    {
        var expectedResult = _fixture.Create<decimal>();
        _statisticsService
            .Setup(s
                => s.GetOpenValueSummary())
            .ReturnsAsync(expectedResult);

        var result = await _controller.GetOpenValueSummary();
        
        result.Value.ShouldBe(expectedResult);
        _statisticsService.Verify(s => s.GetOpenValueSummary(), Times.Once);
    }
    
    [Fact]
    public async Task ClosedValueSummary_ReturnsSummary()
    {
        var expectedResult = _fixture.Create<decimal>();
        _statisticsService
            .Setup(s
                => s.GetClosedValueSummary())
            .ReturnsAsync(expectedResult);

        var result = await _controller.GetClosedValueSummary();
        
        result.Value.ShouldBe(expectedResult);
        _statisticsService.Verify(s => s.GetClosedValueSummary(), Times.Once);
    }
}