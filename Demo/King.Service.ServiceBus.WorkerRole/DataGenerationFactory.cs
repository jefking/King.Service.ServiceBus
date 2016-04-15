namespace King.Service.WorkerRole
{
    using System.Collections.Generic;
    using King.Service.WorkerRole.Queue;
    using King.Service.WorkerRole.Topic;
    using ServiceBus;

    //For Demo Purposes
    public class DataGenerationFactory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            return new IRunnable[]
            {
                //Simulate messages being added to queues
                new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "Event"),
                new QueueForAction(new BusQueueSender(config.FactoryQueueName, config.Connection), "Factory"),
                new QueueToShards(new BusQueueShards(config.ShardsQueueName, config.Connection, 10)),
                new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection)),

                //Simulate messages being sent to topics
                new TopicShipper(new BusTopicSender(config.TopicName, config.Connection)),
            };
        }
    }
}