namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using System;

    public class InitializeQueueTask : InitializeTask
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string name = null;

        public InitializeQueueTask(string name, string connection)
            : this(new BusManagementClient(connection), name)
        {
        }

        public InitializeQueueTask(IBusManagementClient client, string name)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }

            this.client = client;
            this.name = name;
        }

        public override void Run()
        {
            var exists = client.QueueExists(name).Result;
            if (!exists)
            {
                client.QueueCreate(name).Wait();
            }
        }
    }
}