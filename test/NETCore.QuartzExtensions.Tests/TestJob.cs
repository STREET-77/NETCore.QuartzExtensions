using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.QuartzExtensions.Tests
{
    public class TestJob : IJob
    {
        public TestJob(IConfiguration configuration)
        {
            Console.WriteLine(configuration["Quartz:ThreadCount"] ?? "未设置配置文件路径");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Job Execute.");
            await Task.CompletedTask;
        }
    }
}
