﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace NETCore.QuartzExtensions
{
    public class QuartzSchedulerFactory
    {
        private readonly ILogger _logger;
        private readonly IJobFactory _jobFactory;
        private readonly QuartzOptions _options;
        private IScheduler _scheduler;

        public QuartzSchedulerFactory(ILogger<QuartzSchedulerFactory> logger
            , IJobFactory jobFactory
            , IOptions<QuartzOptions> options)
        {
            _logger = logger;
            _jobFactory = jobFactory;
            if (options != null)
                _options = options.Value;
        }

        public ISchedulerFactory StdSchedulerFactory
        {
            get
            {
                return new Lazy<ISchedulerFactory>(() =>
                {
                    var properties = new NameValueCollection
                    {
                        ["quartz.scheduler.instanceName"] = _options.SchedulerInstanceName,
                        ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                        ["quartz.threadPool.threadCount"] = _options.ThreadCount.ToString(),
                        ["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins",
                        ["quartz.plugin.xml.fileNames"] = _options.XmlFilePath,
                        ["quartz.plugin.xml.FailOnFileNotFound"] = "true",
                        ["quartz.plugin.xml.failOnSchedulingError"] = "false"
                    };

                    return new StdSchedulerFactory(properties);

                }).Value;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _scheduler = await StdSchedulerFactory.GetScheduler();
                // 设置IJob实现类的获取方式（DI）
                _scheduler.JobFactory = _jobFactory;

                await _scheduler.Start(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public async Task Shutdown(CancellationToken cancellationToken = default)
        {
            await _scheduler.Shutdown(cancellationToken);
        }

        public async Task Shutdown(bool waitForJobsToComplete, CancellationToken cancellationToken = default)
        {
            await _scheduler.Shutdown(waitForJobsToComplete, cancellationToken);
        }
    }
}
