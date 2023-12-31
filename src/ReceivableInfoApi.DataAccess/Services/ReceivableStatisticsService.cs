﻿using Microsoft.EntityFrameworkCore;
using ReceivableInfoApi.Common.Services;

namespace ReceivableInfoApi.DataAccess.Services;

public class ReceivableStatisticsService : IReceivableStatisticsService
{
    private readonly DataContext _dbContext;

    public ReceivableStatisticsService(DataContext dbContext) => _dbContext = dbContext;

    public async Task<decimal> GetClosedValueSummary()
        => await _dbContext.Receivables.AsNoTracking().Where(r => r.ClosedDate != null).SumAsync(r => r.PaidValue);

    public async Task<decimal> GetOpenValueSummary()
        => await _dbContext.Receivables.AsNoTracking().Where(r => r.ClosedDate == null).SumAsync(r => r.OpeningValue);
}