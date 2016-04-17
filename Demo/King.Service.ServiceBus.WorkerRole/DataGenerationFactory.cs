namespace King.Service.WorkerRole
{
    using System.Collections.Generic;
    using King.Service.WorkerRole.Queue;
    using ServiceBus;

    //Simulate messages being added to queues
    public class DataGenerationFactory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            return new IRunnable[]
            {
                new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "event through queue"),
                new QueueForAction(new BusQueueSender(config.FactoryQueueName, config.Connection), "factory"),
                new QueueForAction(new BusTopicSender(config.TopicName, config.Connection), "topic"),
                new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection)),
                new QueueToShards(new BusQueueShardSender(config.ShardsQueueName, config.Connection, config.ShardsCount)),
            };
        }
    }
}