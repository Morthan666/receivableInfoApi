using Microsoft.EntityFrameworkCore;
using ReceivableInfoApi.Common.Model;
using ReceivableInfoApi.Common.Services;

namespace ReceivableInfoApi.DataAccess;

public class ReceivableCRUDService  : IReceivableCRUDService
{
    private DataContext _dbContext;

    public ReceivableCRUDService(DataContext dbContext) => _dbContext = dbContext;

    public async Task<Receivable> Get(string reference) => await _dbContext.Receivables.SingleOrDefaultAsync(r => r.Reference.Equals(reference));

    public async Task<Receivable[]> GetAll() => await _dbContext.Receivables.ToArrayAsync();
    
    public async Task<bool> Create(Receivable receivable)
    {
        var existing = await Get(receivable.Reference);

        if (existing is null) _dbContext.Receivables.Add(receivable);
        else _dbContext.Receivables.Update(receivable);
        await _dbContext.SaveChangesAsync();

        return existing is null;
    }

    public async Task Update(Receivable receivable)
    {
        _dbContext.Receivables.Update(receivable);

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(string reference)
    {
        var receivable = await Get(reference);
        
        if (receivable != null) _dbContext.Receivables.Remove(receivable);

        await _dbContext.SaveChangesAsync();
    }
}