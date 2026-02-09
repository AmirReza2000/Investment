using Hangfire;
using Microsoft.Extensions.Hosting;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Investment.Infrastructure.Services.Contract;

public class ContractJob : IHostedService
{
    private readonly IRecurringJobManager _recurringJobManager;

    private readonly string _jobGuid = "8F7C2925-6C97-4275-8930-8458894F841A";

    public ContractJob(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _recurringJobManager.AddOrUpdate<ContractProfitAllocator>(
            _jobGuid,
             it => it.AllocateAsync(cancellationToken),
            Cron.Daily);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}