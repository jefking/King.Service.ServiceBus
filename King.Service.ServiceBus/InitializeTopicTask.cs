namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using System;
    using System.Threading.Tasks;

    public class InitializeTopicTask : InitializeTask
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string name = null;

        public InitializeTopicTask(string name, string connection)
            : this(new BusManagementClient(connection), name)
        {
        }

        public InitializeTopicTask(IBusManagementClient client, string name)
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
            var exists = await client.TopicExists(name);
            if (!exists)
            {
                await client.TopicCreate(name);
            }
        }
    }
}