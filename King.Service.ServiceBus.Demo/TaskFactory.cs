namespace King.Service.ServiceBus.Demo
{
    using System.Collections.Generic;

    public class TaskFactory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig passthrough)
        {
            yield return null;
        }
    }
}