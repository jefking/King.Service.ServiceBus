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
            var queue = new BusQueueClient(config.ConnectionString, config.QueueName);
            var topic = new BusTopicClient(config.ConnectionString, config.TopicName);
            var subscription = new BusSubscriptionClient(config.ConnectionString, config.TopicName, config.Subscription);

            // Initialize Tasks
            yield return new InitializeQueueTask(config.ConnectionString, config.QueueName);
            yield return new InitializeTopicTask(config.ConnectionString, config.TopicName);
            yield return new InitializeSubscriptionTask(config.ConnectionString, config.TopicName, config.Subscription);

            // Compute Tasks
            yield return new CompanyQueuer(queue);
            yield return new CompanyDequeuer(queue);

            yield return new EmployeeQueuer(topic);
            yield return new EmployeeDequeuer(subscription);
        }
    }
}