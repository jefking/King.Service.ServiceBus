namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Queue;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
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
            var manager = NamespaceManager.CreateFromConnectionString(config.Connection);

            //Connection
            var pollingClient = new BusQueue(config.PollingName, config.Connection);
            var eventClient = new BusQueue(config.EventsName, config.Connection);

            //InitializationL Polling
            yield return new InitializeBusQueue(pollingClient);

            //Initialization: Events
            yield return new InitializeBusQueue(eventClient);

            //Load polling dequeue object to run
            var dequeue = new BusDequeue<ExampleModel>(pollingClient, new ExampleProcessor());

            //Polling Dequeue Runner
            yield return new AdaptiveRunner(dequeue);

            //Task for watching for queue events
            yield return new BusEvents<ExampleModel>(eventClient, new EventHandler());

            //Tasks for queuing work
            yield return new QueueForAction(pollingClient, "Poll");
            yield return new QueueForAction(eventClient, "Event");
        }
    }
}