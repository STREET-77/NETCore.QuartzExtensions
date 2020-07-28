## NETCore.QuartzExtensions

实现QuartzNet单机XML的NetCore封装，目的是能够让QuartzNet在NetCore环境拿来就用。数据库持久化方式（集群）请参考[QuartzNet](https://github.com/quartznet/quartznet/tree/master/database/tables)。

#### 使用方式

1. 项目引用 `NETCore.QuartzExtensions` 

2. 项目根目录下引入XML配置文件（可手动配置文件位置）

   ```xml
   <?xml version="1.0" encoding="UTF-8"?>
   <job-scheduling-data xmlns="http://quartznet.sourceforge.net/JobSchedulingData"
           xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    				version="2.0">
   	<processing-directives>
   		<overwrite-existing-data>true</overwrite-existing-data>
   	</processing-directives>
   	<schedule>
   		<job>
   			<name>jobName1</name>
   			<group>jobGroup1</group>
   			<description>job description</description>
               <!-- Job类型,命名空间 -->
   			<job-type>NETCore.QuartzExtensions.Tests.TestJob, NETCore.QuartzExtensions.Tests</job-type>
   			<durable>true</durable>
   			<recover>false</recover>
   		</job>
   		<job>
   			<name>jobName2</name>
   			<group>jobGroup1</group>
   			<description>job description</description>
   			<job-type>NETCore.QuartzExtensions.Tests.TestJob2, NETCore.QuartzExtensions.Tests</job-type>
   			<durable>true</durable>
   			<recover>false</recover>
   		</job>
   		<trigger>
   			<simple>
   				<name>simpleName</name>
   				<group>simpleGroup</group>
   				<description>SimpleTriggerDescription</description>
   				<job-name>jobName1</job-name>
   				<job-group>jobGroup1</job-group>
   				<start-time>1982-06-28T18:15:00.0Z</start-time>
   				<misfire-instruction>SmartPolicy</misfire-instruction>
                   <!-- 重复次数 -->
                   <!-- -1:不限制 -->
   				<repeat-count>-1</repeat-count>
                   <!-- 重复间隔时间(毫秒级) -->
   				<repeat-interval>5000</repeat-interval>
   			</simple>
   		</trigger>
   		<trigger>
   			<cron>
   				<name>cronName</name>
   				<group>cronGroup</group>
   				<job-name>jobName2</job-name>
   				<job-group>jobGroup1</job-group>
   				<start-time>2020-07-16T16:20:00+08:00</start-time>
                   <!-- 每10秒执行一次 -->
   				<cron-expression>0/10 * * * * ?</cron-expression>
   			</cron>
   		</trigger>
   	</schedule>
   </job-scheduling-data>
   
   ```

3. 继承`IJob`实现业务逻辑

   ```c#
   public class TestJob : IJob
   {
       public async Task Execute(IJobExecutionContext context)
       {
           // 业务逻辑
           await Task.CompletedTask;
       }
   }
   ```

4. 服务注入Quartz

   ```c#
   var hostBuilder = Host.CreateDefaultBuilder()
                   .ConfigureServices(services =>
                   {
                       // 注入Quartz
                       // 多个任务直接注入使用
                       services.AddQuartz<TestJob>()
                           .AddScoped<TestJob2>();
                   })
   ```

5. 在构造函数中获取`QuartzSchedulerFactory` 

   ```c#
   public class TestHostService : IHostedService
   {
       private readonly QuartzSchedulerFactory _quartzSchedulerFactory;
   
       public HostService(QuartzSchedulerFactory quartzSchedulerFactory)
       {
           _quartzSchedulerFactory = quartzSchedulerFactory;
       }
   
       public async Task StartAsync(CancellationToken cancellationToken)
       {
           // 启动任务
           await _quartzSchedulerFactory.StartAsync(cancellationToken);
       }
   
       public async Task StopAsync(CancellationToken cancellationToken)
       {
           // 停止任务
           await _quartzSchedulerFactory.Shutdown(cancellationToken);
       }
   }
   ```

   

