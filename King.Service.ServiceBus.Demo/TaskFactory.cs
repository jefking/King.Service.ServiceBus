namespace King.Service.ServiceBus.Demo
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using King.Service.ServiceBus.Demo.Processors;
    using King.Service.ServiceBus.Demo.Tasks;
    using System.Collections.Generic;

    public class TaskFactory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
        {
            //Setup general queue client (send/recieve)
            var client = new BusQueueClient(config.QueueName, config.ConnectionString);

            yield return new CompanyQueuer(client);
            yield return new CompanyDequeuer(client);
        }
    }
}