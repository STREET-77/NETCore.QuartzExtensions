using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCore.QuartzExtensions.Tests
{
    public class HostService : IHostedService
    {
        private readonly QuartzSchedulerFactory _quartzSchedulerFactory;

        public HostService(QuartzSchedulerFactory quartzSchedulerFactory)
        {
            _quartzSchedulerFactory = quartzSchedulerFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _quartzSchedulerFactory.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _quartzSchedulerFactory.Shutdown(cancellationToken);
        }
    }
}
