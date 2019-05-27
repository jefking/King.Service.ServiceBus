namespace King.Service.ServiceBus.Demo
{
    using Microsoft.Azure.ServiceBus;
    using King.Service.Data;
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
            var employees = new BusSubscriptionClient(config.ConnectionString, config.TopicName, "all");
            var rademployees = new BusSubscriptionClient(config.ConnectionString, config.TopicName, "top-earners");

            // Initialize Tasks
            yield return new InitializeQueueTask(config.ConnectionString, config.CompanyQueueName);
            yield return new InitializeQueueTask(config.ConnectionString, config.AtQueueName);
            yield return new InitializeStorageTask(new InitializeTopic(config.ConnectionString, config.TopicName));
            yield return new InitializeSubscriptionTask(config.ConnectionString, config.TopicName, employees.Client.SubscriptionName);
            yield return new InitializeSubscriptionTask(config.ConnectionString, config.TopicName, rademployees.Client.SubscriptionName);
            yield return new InitializeRuleTask(config.ConnectionString, config.TopicName, rademployees.Client.SubscriptionName, "top-earners", new SqlFilter("salary >= 500"));

            // Compute Tasks
            yield return new CompanyQueuer(companyClient);
            yield return new BusEvents<CompanyModel>(companyClient, new CompanyProcessor());

            yield return new EmployeeQueuer(topic);
            yield return new BusEvents<EmployeeModel>(employees, new EmployeeProcessor(false));
            yield return new BusEvents<EmployeeModel>(rademployees, new EmployeeProcessor(true));

            yield return new AtQueuer(atClient);
            yield return new BufferedReciever<AtModel>(atClient, new AtProcessor());
        }
    }
}