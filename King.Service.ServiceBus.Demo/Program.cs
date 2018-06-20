namespace King.Service.ServiceBus.Demo
{
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new AppConfig()
            {
                ConnectionString = "",
            };

            using (var manager = new RoleTaskManager<AppConfig>(new TaskFactory()))
            {
                manager.OnStart(config);

                manager.Run();

                while (true)
                {
                    Thread.Sleep(1500);
                }
            }
        }
    }
}