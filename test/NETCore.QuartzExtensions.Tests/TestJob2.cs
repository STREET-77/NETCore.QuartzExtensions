using Quartz;
using System;
using System.Threading.Tasks;

namespace NETCore.QuartzExtensions.Tests
{
    public class TestJob2 : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Job2 Execute.");
            await Task.CompletedTask;
        }
    }
}
