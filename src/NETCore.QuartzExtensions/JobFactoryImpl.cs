using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.QuartzExtensions
{
    /// <summary>
    /// 通过实现IJobFactory，使得IJob实现类可使用构造函数注入方式获取其他依赖
    /// 参考：https://github.com/quartznet/quartznet/blob/v3.0.7/src/Quartz/Simpl/SimpleJobFactory.cs
    /// </summary>
    public class JobFactoryImpl : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactoryImpl(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
