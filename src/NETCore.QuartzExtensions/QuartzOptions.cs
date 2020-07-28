using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.QuartzExtensions
{
    public class QuartzOptions : IOptions<QuartzOptions>
    {
        public string SchedulerInstanceName { get; set; } = "XmlConfiguredInstance";

        /// <summary>
        /// 设置线程池的最大线程数量
        /// 默认：5
        /// </summary>
        public int ThreadCount { get; set; } = 5;

        /// <summary>
        /// quartz_jobs.xml文件路径
        /// 默认：~/quartz_jobs.xml
        /// </summary>
        public string XmlFilePath { get; set; } = "~/quartz_jobs.xml";

        public QuartzOptions Value
        {
            get
            {
                return this;
            }
        }
    }
}
