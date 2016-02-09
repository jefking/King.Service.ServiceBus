namespace King.Service.WorkerRole
{
    using King.Service.ServiceBus;
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        private readonly IRoleTaskManager<Configuration> manager = new RoleTaskManager<Configuration>(new ITaskFactory<Configuration>[] { new Factory(), new DataGenerationFactory()});

        public override void Run()
        {
            this.manager.Run();

            base.Run();
        }

        public override bool OnStart()
        {
            var config = new Configuration()
            {
                EventsName = "events",
                BufferedEventsName = "buffered",
                TopicName = "topic",
                FactoryQueueName = "factory",
                ShardsQueueName = "shards",
                Connection = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString"),
            };

            return this.manager.OnStart(config);
        }

        public override void OnStop()
        {
            this.manager.OnStop();

            base.OnStop();
        }
    }
}