namespace King.Service.ServiceBus.Demo
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Write out standard out
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            // Load Config
            var config = new AppConfig
            {
                ConnectionString = args[0],
                CompanyQueueName = "companies",
                AtQueueName = "always",
                TopicName = "employees"
            };

            // Start tasks
            using (var manager = new RoleTaskManager<AppConfig>(new TaskFactory()))
            {
                // On Start
                manager.OnStart(config);

                // Run
                manager.Run();
                
                // Pin loading thread
                while (true)
                {
                    Thread.Sleep(1500);
                }
            }
        }
    }
}