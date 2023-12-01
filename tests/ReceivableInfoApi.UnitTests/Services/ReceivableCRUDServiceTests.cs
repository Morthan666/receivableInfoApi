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

public class ReceivableCRUDServiceTests
{
    private readonly Fixture _fixture;
    private readonly DataContext _dataContext;
    private readonly IReceivableCRUDService _service;

    public ReceivableCRUDServiceTests()
    {
        _fixture = new Fixture();
        var configuration = new Mock<IConfiguration>();
        _dataContext = new DataContext(configuration.Object);
        _service = new ReceivableCRUDService(_dataContext);
    }

    [Fact]
    public async Task GetAll_GetsAllReceivables()
    {
        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        await _dataContext.AddRangeAsync(receivables);
        await _dataContext.SaveChangesAsync();

        var result = await _service.GetAll();
        
        receivables.All(result.Contains).ShouldBeTrue();
    }
    
    [Fact]
    public async Task Get_GetsSelectedReceivable()
    {
        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        await _dataContext.AddRangeAsync(receivables);
        await _dataContext.SaveChangesAsync();
        var receivable = receivables.First();

        var result = await _service.Get(receivable.Reference);
        
        result.ShouldBeEquivalentTo(receivable);
    }
    
    [Fact]
    public async Task Create_AddsAReceivable()
    {
        var receivable = _fixture.Create<Receivable>();

        var result = await _service.Create(receivable);
        
        result.ShouldBeTrue();
        
        _dataContext.Receivables.ShouldContain(receivable);
    }
    
    [Fact]
    public async Task Create_ReturnsFalseWhenExists()
    {
        var receivable = _fixture.Create<Receivable>();
        _dataContext.Add(receivable);
        await _dataContext.SaveChangesAsync();

        var result = await _service.Create(receivable);
        
        result.ShouldBeFalse();
    }
    
    [Fact]
    public async Task Update_CreatesWhenDoesntExist()
    {
        var receivable = _fixture.Create<Receivable>();

        var result = await _service.Update(receivable);
        
        result.ShouldBeFalse();
        var record = _dataContext.Receivables.AsNoTracking().Single(r => r.Reference.Equals(receivable.Reference));
        record.ShouldBeEquivalentTo(receivable);
    }
}