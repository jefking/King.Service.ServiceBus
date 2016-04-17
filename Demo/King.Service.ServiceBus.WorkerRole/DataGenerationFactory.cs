namespace King.Service.WorkerRole
{
    using System.Collections.Generic;
    using King.Service.WorkerRole.Queue;
    using King.Service.WorkerRole.Topic;
    using ServiceBus;

    public class DataGenerationFactory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            return new IRunnable[]
            {
                //Simulate messages being added to queues
                new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "event through queue"),
                new QueueForAction(new BusQueueSender(config.FactoryQueueName, config.Connection), "factory"),
                new QueueToShards(new BusQueueShards(config.ShardsQueueName, config.Connection, config.ShardsCount)),
                new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection)),

                //Simulate messages being sent to topics
                new TopicShipper(new BusTopicSender(config.TopicName, config.Connection)),
            };
        }
    }
}