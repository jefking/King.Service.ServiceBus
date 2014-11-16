namespace King.Service.ServiceBus
{
    using King.Service.Data;
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Queue;
    using Microsoft.ServiceBus.Messaging;
    using System.Collections.Generic;
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus;

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
            var pollingClient = QueueClient.Create(config.PollingName);
            var eventClient = QueueClient.Create(config.EventsName);

            //InitializationL Polling
            yield return new InitializeQueue(config.PollingName, manager);

            //Initialization: Events
            yield return new InitializeQueue(config.EventsName, manager);

            //Load polling dequeue object to run
            var dequeue = new BusDequeue<ExampleModel>(pollingClient, new ExampleProcessor());

            //Polling Dequeue Runner
            yield return new AdaptiveRunner(dequeue);

            //Task for watching for queue events
            yield return new BusEvents<ExampleModel>(eventClient, new EventHandler());

            //Tasks for queuing work
            yield return new QueueForPoll(pollingClient);
            yield return new QueueForEvents(eventClient);
        }
    }
}
