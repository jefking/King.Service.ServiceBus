namespace King.Service.ServiceBus.Demo
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using King.Service.ServiceBus.Demo.Models;
    using King.Service.ServiceBus.Demo.Processors;
    using King.Service.ServiceBus.Demo.Tasks;
    using System.Collections.Generic;

    public class TaskFactory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
        {
            //Setup general queue client (send/recieve)
            var companyClient = new BusQueueClient(config.ConnectionString, config.CompanyQueueName);
            var atClient = new BusQueueClient(config.ConnectionString, config.AtQueueName);
            var topic = new BusTopicClient(config.ConnectionString, config.TopicName);
            var subscription = new BusSubscriptionClient(config.ConnectionString, config.TopicName, config.Subscription);

            // Initialize Tasks
            yield return new InitializeQueueTask(config.ConnectionString, config.CompanyQueueName);
            yield return new InitializeQueueTask(config.ConnectionString, config.AtQueueName);
            yield return new InitializeTopicTask(config.ConnectionString, config.TopicName);
            yield return new InitializeSubscriptionTask(config.ConnectionString, config.TopicName, config.Subscription);

            // Compute Tasks
            yield return new CompanyQueuer(companyClient);
            yield return new BusEvents<CompanyModel>(companyClient, new CompanyProcessor());

            yield return new EmployeeQueuer(topic);
            yield return new BusEvents<EmployeeModel>(subscription, new EmployeeProcessor());

            yield return new AtQueuer(atClient);
            yield return new BufferedReciever<AtModel>(atClient, new AtProcessor());
        }
    }
}