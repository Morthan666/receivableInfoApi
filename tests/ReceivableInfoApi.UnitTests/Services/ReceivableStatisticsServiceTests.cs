using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using ReceivableInfoApi.Common.Model;
using ReceivableInfoApi.Common.Services;
using ReceivableInfoApi.DataAccess;
using ReceivableInfoApi.DataAccess.Services;
using Shouldly;

namespace ReceivableInfoApi.UnitTests.Services;

public class ReceivableStatisticsServiceTests
{
    private readonly Fixture _fixture;
    private readonly DataContext _dataContext;
    private readonly IReceivableStatisticsService _service;

    public ReceivableStatisticsServiceTests()
    {
        _fixture = new Fixture();
        var configuration = new Mock<IConfiguration>();
        _dataContext = new DataContext(configuration.Object);
        _service = new ReceivableStatisticsService(_dataContext);
    }
    
    [Fact]
    public async Task GetOpenValueSummary_ReturnsSummary()
    {
        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        await _dataContext.AddRangeAsync(receivables);
        await _dataContext.SaveChangesAsync();

        var result = await _service.GetOpenValueSummary();

        var expected = await _dataContext.Receivables.AsNoTracking()
            .Where(r => r.ClosedDate == null).SumAsync(r => r.OpeningValue);
        result.ShouldBe(expected);
    }
    
    [Fact]
    public async Task GetClosedValueSummary_ReturnsSummary()
    {
        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        await _dataContext.AddRangeAsync(receivables);
        await _dataContext.SaveChangesAsync();

        var result = await _service.GetClosedValueSummary();

        var expected = await _dataContext.Receivables.AsNoTracking()
            .Where(r => r.ClosedDate != null).SumAsync(r => r.PaidValue);
        result.ShouldBe(expected);
    }
}