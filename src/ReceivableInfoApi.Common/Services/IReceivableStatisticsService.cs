namespace ReceivableInfoApi.Common.Services;

public interface IReceivableStatisticsService
{
    Task<decimal> GetClosedValueSummary();
    Task<decimal> GetOpenValueSummary();
}