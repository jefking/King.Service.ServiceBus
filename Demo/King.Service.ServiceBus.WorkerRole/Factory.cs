namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service.Data;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Models;
    using King.Service.WorkerRole.Queue;
    using Queue;

    public class Factory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            var factory = new BusDequeueFactory(config.Connection);

            var tasks = new List<IRunnable>(new IRunnable[] {

                //Initialize Service Bus
                new InitializeStorageTask(new BusQueue(config.EventsName, config.Connection)),
                new InitializeStorageTask(new BusQueue(config.BufferedEventsName, config.Connection)),
                new InitializeStorageTask(new BusTopic(config.TopicName, config.Connection)),
                new InitializeStorageTask(new BusTopicSubscription(config.TopicName, config.Connection, config.TopicSubscriptionName, config.TopicSubscriptionSqlFilter)),
                new InitializeStorageTask(new BusHub(config.HubName, config.Connection)),

                //Events
                new BusEvents<ExampleModel>(new BusQueueReciever(config.EventsName, config.Connection), new EventHandler()),
                new BusEvents<ExampleModel>(new BusSubscriptionReciever(config.TopicName, config.Connection, config.TopicSubscriptionName), new EventHandler()),

                //Task for recieving queue events to specific times
                new BufferedReciever<ExampleModel>(new BusQueueReciever(config.BufferedEventsName, config.Connection), new EventHandler()),
                new BufferedReciever<ExampleModel>(new BusSubscriptionReciever(config.BufferedEventsName, config.Connection, ""), new EventHandler()),
            });

            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            tasks.AddRange(factory.Dequeue<ExampleProcessor, ExampleModel>(config.FactoryQueueName));
            tasks.AddRange(factory.Shards<ExampleProcessor, ExampleModel>(config.ShardsQueueName, config.ShardsCount));

            return tasks;
        }
    }
}