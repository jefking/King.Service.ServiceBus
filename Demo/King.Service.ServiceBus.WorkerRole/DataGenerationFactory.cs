namespace King.Service.WorkerRole
{
    using King.Service.WorkerRole.Queue;
    using King.Service.WorkerRole.Topic;
    using ServiceBus;
    using System.Collections.Generic;

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
                new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection)),

                //Simulate messages being sent to topics
                new TopicShipper(new TopicSender(config.TopicName, config.Connection)),
            };
        }
    }
}
