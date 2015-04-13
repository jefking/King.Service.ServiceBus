namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Queue;
    using King.Service.WorkerRole.Scalable;
    using King.Service.WorkerRole.Topic;
    using System.Collections.Generic;

    /// <summary>
    /// Factory
    /// </summary>
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
            var pollReceiver = new BusQueueReciever(config.PollingName, config.Connection);
            var scalingQueue = new BusQueueReciever(config.ScalingQueueName, config.Connection);
            var eventReciever = new BusQueueReciever(config.EventsName, config.Connection);
            var bufferReciever = new BusQueueReciever(config.BufferedEventsName, config.Connection);
            var dynamicReciever = new BusQueueReciever(config.DynamicQueueName, config.Connection);

            //Initialize Service Bus
            yield return new InitializeBusQueue(pollReceiver);
            yield return new InitializeBusQueue(scalingQueue);
            yield return new InitializeBusQueue(eventReciever);
            yield return new InitializeBusQueue(bufferReciever);
            yield return new InitializeBusQueue(dynamicReciever);
            yield return new InitializeTopic(config.TopicName, config.Connection);

            //Polling Dequeue Runner
            yield return new AdaptiveRunner(new BusDequeue<ExampleModel>(pollReceiver, new ExampleProcessor()));

            //Task for watching for queue events
            yield return new BusEvents<ExampleModel>(eventReciever, new EventHandler());

            //Task for recieving queue events to specific times
            yield return new BufferedReciever<ExampleModel>(bufferReciever, new EventHandler());

            //Auto Scaling Dequeue Task
            yield return new ScalableQueue(scalingQueue, config);

            //Auto Batch Size Dequeue Task
            yield return new AdaptiveRunner(new BusDequeueBatchDynamic<ExampleModel>(config.DynamicQueueName, config.Connection, new ExampleProcessor()));

            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            var f = new BusDequeueFactory<ExampleModel>();
            foreach (var t in f.Tasks(new SetupExample(config)))
            {
                yield return t;
            }

            //Simulate messages being added to queues
            yield return new QueueForAction(new BusQueueSender(config.PollingName, config.Connection), "Poll");
            yield return new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "Event");
            yield return new QueueForAction(new BusQueueSender(config.ScalingQueueName, config.Connection), "Scaling");
            yield return new QueueForAction(new BusQueueSender(config.DynamicQueueName, config.Connection), "Dynamic");
            yield return new QueueForAction(new BusQueueSender(config.FactoryQueueName, config.Connection), "Factory");
            yield return new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection));

            //Simulate messages being sent to topics
            yield return new TopicShipper(new TopicSender(config.TopicName, config.Connection));
        }
    }
}