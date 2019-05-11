namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using System;
    using System.Threading.Tasks;

    public class InitializeQueueTask : InitializeTask
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string name = null;

        public InitializeQueueTask(string connection, string name)
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

        public override async Task RunAsync()
        {
            var exists = await client.QueueExists(name);
            if (!exists)
            {
                await client.QueueCreate(name);
            }
        }
    }
}