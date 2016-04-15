namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service.Data;
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Models;
    using King.Service.WorkerRole.Queue;

    public class Factory : ITaskFactory<Configuration>
    {
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough">Configuration</param>
        /// <returns></returns>
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            var factory = new BusDequeueFactory(config.Connection);

            //Tasks
            var tasks = new List<IRunnable>(new IRunnable[] {

                //Initialize Service Bus
                new InitializeStorageTask(new BusQueue(config.EventsName, config.Connection)),
                new InitializeStorageTask(new BusQueue(config.BufferedEventsName, config.Connection)),
                new InitializeStorageTask(new BusTopic(config.TopicName, config.Connection)),
                new InitializeStorageTask(new BusHub(config.HubName, config.Connection)),

                //Task for watching for queue events
                new BusEvents<ExampleModel>(new BusQueueReciever(config.EventsName, config.Connection), new EventHandler()),

                //Task for recieving queue events to specific times
                new BufferedReciever<ExampleModel>(new BusQueueReciever(config.BufferedEventsName, config.Connection), new EventHandler()),
            });

            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            tasks.AddRange(factory.Dequeue<ExampleProcessor, ExampleModel>(config.FactoryQueueName, QueuePriority.Medium));
            tasks.AddRange(factory.Shards<ExampleProcessor, ExampleModel>(config.ShardsQueueName, 10));

            return tasks;
        }
    }
}