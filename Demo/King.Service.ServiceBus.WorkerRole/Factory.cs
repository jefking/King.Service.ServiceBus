namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Queue;
    using System.Collections.Generic;

    /// <summary>
    /// Facotry
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
            var pollSender = new BusQueueSender(config.PollingName, config.Connection);
            var eventReciever = new BusQueueReciever(config.EventsName, config.Connection);
            var eventSender = new BusQueueSender(config.EventsName, config.Connection);
            var bufferReciever = new BusQueueReciever(config.BufferedEventsName, config.Connection);
            var bufferSender = new BusQueueSender(config.BufferedEventsName, config.Connection);

            //Initialize Polling
            yield return new InitializeBusQueue(pollReceiver);
            yield return new InitializeBusQueue(eventSender);
            yield return new InitializeBusQueue(bufferReciever);

            //Load polling dequeue object to run
            var dequeue = new BusDequeue<ExampleModel>(pollReceiver, new ExampleProcessor());

            //Polling Dequeue Runner
            //yield return new AdaptiveRunner(dequeue);

            //Task for watching for queue events
            //yield return new BusEvents<ExampleModel>(eventReciever, new EventHandler());

            //Task for recieving queue events to specific times
            yield return new BufferedReciever<ExampleModel>(bufferReciever, new EventHandler());

            //Tasks for queuing work
            yield return new QueueForAction(pollSender, "Poll");
            yield return new QueueForAction(eventSender, "Event");
            yield return new QueueForAction(bufferSender, "Buffer");
        }
    }
}