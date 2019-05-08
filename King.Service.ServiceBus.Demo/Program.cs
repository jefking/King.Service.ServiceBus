namespace King.Service.ServiceBus.Demo
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            foreach (var arg in args)
            {
                Trace.TraceInformation("arg: {0}", arg);
            }

            // Load Config
            var config = new AppConfig
            {
                ConnectionString = args[0],
                QueueName = "company"
            };

            Trace.TraceInformation("Connection String: {0}", config.ConnectionString);

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