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
            //Connection
            var pollReceiver = new BusQueueReciever(config.PollingName, config.Connection);
            var pollSender = new BusQueueSender(config.PollingName, config.Connection);
            var eventReciever = new BusQueueReciever(config.EventsName, config.Connection);
            var eventSender = new BusQueueSender(config.EventsName, config.Connection);

            //InitializationL Polling
            yield return new InitializeBusQueue(pollReceiver);

            //Initialization: Events
            yield return new InitializeBusQueue(eventSender);

            //Load polling dequeue object to run
            var dequeue = new BusDequeue<ExampleModel>(pollReceiver, new ExampleProcessor());

            //Polling Dequeue Runner
            yield return new AdaptiveRunner(dequeue);

            //Task for watching for queue events
            yield return new BusEvents<ExampleModel>(eventReciever, new EventHandler());

            //Tasks for queuing work
            yield return new QueueForAction(pollSender, "Poll");
            yield return new QueueForAction(eventSender, "Event");
        }
    }
}