namespace King.Service.ServiceBus
{
    using King.Service.Data;
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Models;
    using King.Service.WorkerRole.Queue;
    using King.Service.WorkerRole.Topic;
    using System.Collections.Generic;

    public class Factory : ITaskFactory<Configuration>
    {
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough"></param>
        /// <returns></returns>
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            var tasks = new List<IRunnable>();

            //Connections
            var eventReciever = new BusQueueReciever(config.EventsName, config.Connection);
            var bufferReciever = new BusQueueReciever(config.BufferedEventsName, config.Connection);

            //Initialize Service Bus
            tasks.Add(new InitializeBusQueue(eventReciever));
            tasks.Add(new InitializeBusQueue(bufferReciever));
            tasks.Add(new InitializeTopic(config.TopicName, config.Connection));

            //Task for watching for queue events
            tasks.Add(new BusEvents<ExampleModel>(eventReciever, new EventHandler()));

            //Task for recieving queue events to specific times
            tasks.Add(new BufferedReciever<ExampleModel>(bufferReciever, new EventHandler()));
            
            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            var factory = new BusDequeueFactory(config.Connection);
            tasks.AddRange(factory.Dequeue<ExampleProcessor, ExampleModel>(config.FactoryQueueName, QueuePriority.Medium));

            //Simulate messages being added to queues
            tasks.Add(new QueueForAction(new BusQueueSender(config.PollingName, config.Connection), "Poll"));
            tasks.Add(new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "Event"));
            tasks.Add(new QueueForAction(new BusQueueSender(config.FactoryQueueName, config.Connection), "Factory"));
            tasks.Add(new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection)));

            //Simulate messages being sent to topics
            tasks.Add(new TopicShipper(new TopicSender(config.TopicName, config.Connection)));

            return tasks;
        }
    }
}