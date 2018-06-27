namespace King.Service.ServiceBus.Demo
{
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Appication Configuration Settings
            var config = new AppConfig()
            {
                ConnectionString = "",
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