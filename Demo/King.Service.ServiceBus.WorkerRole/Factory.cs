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

            //Initialize Service Bus
            yield return new InitializeBusQueue(eventReciever);
            yield return new InitializeBusQueue(bufferReciever);
            yield return new InitializeTopic(config.TopicName, config.Connection);
            
            //Task for watching for queue events
            yield return new BusEvents<ExampleModel>(eventReciever, new EventHandler());

            //Task for recieving queue events to specific times
            yield return new BufferedReciever<ExampleModel>(bufferReciever, new EventHandler());
            
            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            var factory = new BusDequeueFactory(config.Connection);
            var setup = new QueueSetupProcessor<ExampleProcessor, ExampleModel>
            {
                Name = config.FactoryQueueName,
                Priority = QueuePriority.Medium,
            };

            foreach (var t in factory.Tasks<ExampleModel>(setup))
            {
                yield return t;
            }

            //Simulate messages being added to queues
            yield return new QueueForAction(new BusQueueSender(config.PollingName, config.Connection), "Poll");
            yield return new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "Event");
            yield return new QueueForAction(new BusQueueSender(config.FactoryQueueName, config.Connection), "Factory");
            yield return new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection));

            //Simulate messages being sent to topics
            yield return new TopicShipper(new TopicSender(config.TopicName, config.Connection));
        }
    }
}