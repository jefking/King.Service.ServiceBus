namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;

    public class InitializeQueue : InitializeTask
    {
        protected string name;
        protected NamespaceManager manager;
        public InitializeQueue(string name, NamespaceManager manager)
        {
            this.name = name;
            this.manager = manager;
        }

        public override void Run()
        {
            if (manager.QueueExists(name))
            {
            }
            else
            {
                var desc = manager.CreateQueueAsync(name);
            }
        }
    }
}