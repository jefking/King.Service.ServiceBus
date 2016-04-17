namespace King.Service.WorkerRole
{
    using King.Service.ServiceBus;
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        private readonly IRoleTaskManager<Configuration> manager = new RoleTaskManager<Configuration>(new ITaskFactory<Configuration>[] { new Factory(), new DataGenerationFactory() });

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
                BufferedQueueName = "buffered",
                TopicName = "topic",
                TopicSubscriptionName = "subscription",
                //TopicSubscriptionSqlFilter = "",
                HubName = "hub",
                FactoryQueueName = "factory",
                ShardsQueueName = "shards",
                ShardsCount = 3,
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