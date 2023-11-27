using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AutoFixture;
using ReceivableInfoApi.Common.Model;
using Shouldly;

namespace ReceivableInfoApi.IntegrationTests.ControllerTests;

[Collection(HttpServerCollection.Name)]
public class ReceivablesControllerTests
{
    private const string BasePath = "v1/receivables";
    private readonly HttpServerFixture _serverFixture;
    private readonly Fixture _fixture;

    public ReceivablesControllerTests(HttpServerFixture serverFixture)
    {
        _serverFixture = serverFixture;
        _fixture = new Fixture();
    }
    
    [Fact]
    public async Task GetAllGetsAllRecords()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);

        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        foreach (var receivable in receivables)
            await client.PostAsync(
                BasePath,
                new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json"));

        var result = await GetCurrentSet(client);
        
        result.Length.ShouldBe(receivables.Length);
        foreach (var receivable in receivables)
            result.Single(r => r.Reference == receivable.Reference).ShouldBeEquivalentTo(receivable);
    }
    
    [Fact]
    public async Task GetGetsARecordByReference()
    {
        var client = _serverFixture.CreateHttpClient();
        
        var receivable1 = _fixture.Create<Receivable>();
        var receivable2 = _fixture.Create<Receivable>();
        await client.PostAsync(
            BasePath,
            new StringContent(JsonSerializer.Serialize(receivable1), Encoding.UTF8, "application/json"));
        await client.PostAsync(
            BasePath,
            new StringContent(JsonSerializer.Serialize(receivable2), Encoding.UTF8, "application/json"));

        var result1 = await client.GetAsync($"{BasePath}/{receivable1.Reference}");
        var result2 = await client.GetAsync($"{BasePath}/{receivable2.Reference}");
        
        (await result1.Content.ReadFromJsonAsync<Receivable>()).ShouldBeEquivalentTo(receivable1);
        (await result2.Content.ReadFromJsonAsync<Receivable>()).ShouldBeEquivalentTo(receivable2);
    }

    [Fact]
    public async Task PostCreatesANewRecord()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);
        
        var receivable = _fixture.Create<Receivable>();
        var content = new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json");
        var postResponse = await client.PostAsync(BasePath, content);

        var location = postResponse.Headers.Location.ToString();
        location.ShouldBe(receivable.Reference);

        var currentState = await GetCurrentSet(client);
        currentState.Length.ShouldBe(1);
        currentState.Single().ShouldBeEquivalentTo(receivable);
    }
    
    [Fact]
    public async Task PostReturnsSeeOtherWhenRecordExists()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);
        
        var receivable = _fixture.Create<Receivable>();
        var content = new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json");
        await client.PostAsync(BasePath, content);
        
        var currentState = await GetCurrentSet(client);
        currentState.Length.ShouldBe(1);
        currentState.Single().ShouldBeEquivalentTo(receivable);
        
        var postResponse2 = await client.PostAsync(BasePath, content);
        
        postResponse2.StatusCode.ShouldBe(HttpStatusCode.SeeOther);

        var location = postResponse2.Headers.Location.ToString();
        location.ShouldBe(receivable.Reference);
        var currentState2 = await GetCurrentSet(client);
        currentState2.Length.ShouldBe(1);
        currentState2.Single().ShouldBeEquivalentTo(receivable);
    }
    
    [Fact]
    public async Task PutUpdatesARecord()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);

        var receivable = _fixture.Create<Receivable>();
        var createContent = new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json");
        await client.PostAsync(BasePath, createContent);

        var updatedReceivable = _fixture.Create<Receivable>();
        updatedReceivable.Reference = receivable.Reference;
        
        var updateContent = new StringContent(JsonSerializer.Serialize(updatedReceivable), Encoding.UTF8, "application/json");
        await client.PutAsync($"{BasePath}/{receivable.Reference}", updateContent);
        
        var currentState = await GetCurrentSet(client);
        currentState.Length.ShouldBe(1);
        currentState.Single().ShouldBeEquivalentTo(updatedReceivable);
    }
    
    [Fact]
    public async Task PutReturnsBadRequestOnReferenceMismatch()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);

        var receivable = _fixture.Create<Receivable>();
        var createContent = new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json");
        await client.PostAsync(BasePath, createContent);

        var updatedReceivable = _fixture.Create<Receivable>();

        var updateContent = new StringContent(JsonSerializer.Serialize(updatedReceivable), Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{BasePath}/{receivable.Reference}", updateContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task PutCreatesARecordWhenDoesNotExist()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);

        var receivable = _fixture.Create<Receivable>();

        var updateContent = new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json");
        await client.PutAsync($"{BasePath}/{receivable.Reference}", updateContent);
        
        var currentState = await GetCurrentSet(client);
        currentState.Length.ShouldBe(1);
        currentState.Single().ShouldBeEquivalentTo(receivable);
    }
    
    [Fact]
    public async Task DeleteRemovesARecord()
    {
        var client = _serverFixture.CreateHttpClient();

        var receivable = _fixture.Create<Receivable>();
        var createContent = new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json");
        await client.PostAsync(BasePath, createContent);

        await client.DeleteAsync($"{BasePath}/{receivable.Reference}");

        var result = await client.GetAsync($"{BasePath}/{receivable.Reference}");
        
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetOpenValueSummaryGetsSummary()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);

        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        foreach (var receivable in receivables)
            await client.PostAsync(
                BasePath,
                new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json"));

        var response = await client.GetAsync($"{BasePath}/openValueSummary");
        
        var result =  await response.Content.ReadFromJsonAsync<decimal>();
        
        result.ShouldBe(receivables.Where(r => r.ClosedDate == null).Sum(r => r.OpeningValue));
    }
    
    [Fact]
    public async Task GetClosedValueSummaryGetsSummary()
    {
        var client = _serverFixture.CreateHttpClient();
        await CleanSet(client);

        var receivables = _fixture.CreateMany<Receivable>().ToArray();
        foreach (var receivable in receivables)
            await client.PostAsync(
                BasePath,
                new StringContent(JsonSerializer.Serialize(receivable), Encoding.UTF8, "application/json"));

        var response = await client.GetAsync($"{BasePath}/closedValueSummary");
        
        var result =  await response.Content.ReadFromJsonAsync<decimal>();
        
        result.ShouldBe(receivables.Where(r => r.ClosedDate != null).Sum(r => r.PaidValue));
    }
    
    private async Task CleanSet(HttpClient client)
    {
        var currentSet = await GetCurrentSet(client);
        foreach (var receivable in currentSet) await client.DeleteAsync($"{BasePath}/{receivable.Reference}");
    }

    private async Task<Receivable[]> GetCurrentSet(HttpClient client)
    {
        var response = await client.GetAsync(BasePath);
        return response.StatusCode == HttpStatusCode.NoContent 
            ? Array.Empty<Receivable>() 
            : await response.Content.ReadFromJsonAsync<Receivable[]>();
    }
}