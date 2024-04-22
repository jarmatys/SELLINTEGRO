using Quartz;
using Quartz.Impl;
using Sellintegro.SnapshotGenerator.Job.Jobs;

namespace Sellintegro.SnapshotGenerator.Job;

// OTHER SOLUTIONS IDEA: 
// 1. If it is possible, the best solution IMO are cron jobs hosted on kubernetes cluster
// 2. Internal API triggered by Hangfire (or other tool like this)
internal abstract class Program
{
    private static async Task Main(string[] args)
    {
        var isApiAlive = await ApiHealthCheck.IsApiAlive();
        if (!isApiAlive)
        {
            Console.WriteLine("API is not accessible!");
            return;
        }

        // INFO: For task purpose I will use Quartz library (the simples solution but not the best)
        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.Start();

        // var job = JobBuilder.Create<SimpleJsonRecreateJob>()
        //     .WithIdentity("jsonRecreateJob", "group")
        //     .Build();

        var job = JobBuilder.Create<LongPoolingJsonRecreateJob>()
            .WithIdentity("jsonRecreateJob", "group")
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .WithIdentity("trigger", "group")
            .WithCronSchedule("0/10 * * * * ?")
            .Build();

        await scheduler.ScheduleJob(job, trigger);

        Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
        Console.ReadKey();

        await scheduler.Shutdown();
    }
}