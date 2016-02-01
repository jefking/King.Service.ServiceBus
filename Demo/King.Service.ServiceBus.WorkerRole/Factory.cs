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
            //Connections
            var eventReciever = new BusQueueReciever(config.EventsName, config.Connection);
            var bufferReciever = new BusQueueReciever(config.BufferedEventsName, config.Connection);

            var factory = new BusDequeueFactory(config.Connection);
            
            //Tasks
            var tasks = new List<IRunnable>(new IRunnable[] {

                //Initialize Service Bus
                new InitializeBusQueue(eventReciever),
                new InitializeBusQueue(bufferReciever),
                new InitializeTopic(config.TopicName, config.Connection),

                //Task for watching for queue events
                new BusEvents<ExampleModel>(eventReciever, new EventHandler()),

                //Task for recieving queue events to specific times
                new BufferedReciever<ExampleModel>(bufferReciever, new EventHandler()),

                //Simulate messages being added to queues
                new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "Event"),
                new QueueForAction(new BusQueueSender(config.FactoryQueueName, config.Connection), "Factory"),
                new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection)),

                //Simulate messages being sent to topics
                new TopicShipper(new TopicSender(config.TopicName, config.Connection)),
            });

            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            tasks.AddRange(factory.Dequeue<ExampleProcessor, ExampleModel>(config.FactoryQueueName, QueuePriority.Medium));

            return tasks;
        }
    }
}