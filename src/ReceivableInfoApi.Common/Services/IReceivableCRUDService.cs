using ReceivableInfoApi.Common.Model;

namespace ReceivableInfoApi.Common.Services;

public interface IReceivableCRUDService
{
    Task<bool> Create(Receivable receivable);
    Task<bool> Update(Receivable receivable);
    Task<Receivable> Get(string reference);
    Task<Receivable[]> GetAll();
    Task<bool> Delete(string reference);
}