namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Queue;
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
            var eventReciever = new BusQueueReciever(config.EventsName, config.Connection);
            var bufferReciever = new BusQueueReciever(config.BufferedEventsName, config.Connection);

            //Initialize Queues
            yield return new InitializeBusQueue(pollReceiver);
            yield return new InitializeBusQueue(eventReciever);
            yield return new InitializeBusQueue(bufferReciever);

            //Polling Dequeue Runner
            yield return new AdaptiveRunner(new BusDequeue<ExampleModel>(pollReceiver, new ExampleProcessor()));

            //Task for watching for queue events
            yield return new BusEvents<ExampleModel>(eventReciever, new EventHandler());

            //Task for recieving queue events to specific times
            yield return new BufferedReciever<ExampleModel>(bufferReciever, new EventHandler());

            //Simulate messages being added to queues
            yield return new QueueForAction(new BusQueueSender(config.PollingName, config.Connection), "Poll");
            yield return new QueueForAction(new BusQueueSender(config.EventsName, config.Connection), "Event");
            yield return new QueueForBuffer(new BusQueueSender(config.BufferedEventsName, config.Connection));
        }
    }
}