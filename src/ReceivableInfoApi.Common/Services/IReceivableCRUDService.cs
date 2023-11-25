using ReceivableInfoApi.Common.Model;

namespace ReceivableInfoApi.Common.Services;

public interface IReceivableCRUDService
{
    Task<bool> Create(Receivable receivable);
    Task<Receivable> Get(string reference);
    Task<Receivable[]> GetAll();
    Task Update(Receivable receivable);
    Task Delete(string reference);
}