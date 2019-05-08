namespace King.Service.ServiceBus.Demo
{
    using King.Service.ServiceBus.Demo.Tasks;
    using System.Collections.Generic;

    public class TaskFactory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
        {
            yield return new CompanyQueuer(config.QueueName, config.ConnectionString);

            //yield return new RecurringRunner(new CompanyDequeuer(config.QueueName, config.ConnectionString));
        }
    }
}