namespace King.Service.WorkerRole.Queue
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.ServiceBus.Queue;

    public class SetupExample : QueueSetup<ExampleModel>
    {
        public SetupExample(Configuration config)
        {
            ConnectionString = config.Connection;
            Name = config.FactoryQueueName;
            Priority = QueuePriority.Medium;
        }
        public override IProcessor<ExampleModel> Get()
        {
            return new ExampleProcessor();
        }
    }
}