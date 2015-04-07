namespace King.Service.WorkerRole
{
    using King.Service.ServiceBus;
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        private readonly IRoleTaskManager<Configuration> manager = new RoleTaskManager<Configuration>(new Factory());

        public override void Run()
        {
            this.manager.Run();

            base.Run();
        }

        public override bool OnStart()
        {
            var config = new Configuration()
            {
                PollingName = "polling",
                EventsName = "events",
                BufferedEventsName = "buffered",
                TopicName = "topic",
                ScalingQueueName = "scaling",
                DynamicQueueName = "dynamic",
                FactoryQueueName = "factory",
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