namespace King.Service.ServiceBus
{
    using Azure.Data.Wrappers;
    using King.Service.ServiceBus.Wrappers;
    using System;
    using System.Threading.Tasks;

    public class InitializeTopicTask : IAzureStorage
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string name = null;

        public InitializeTopicTask(string connection, string name)
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

        public async Task<bool> CreateIfNotExists()
        {
            var exists = await client.TopicExists(name);
            if (!exists)
            {
                await client.TopicCreate(name);
                return true;
            }

            return false;
        }
    }
}