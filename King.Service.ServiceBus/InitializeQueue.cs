namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using System;
    using System.Threading.Tasks;

    public class InitializeQueue : IAzureStorage
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string name = null;

        public InitializeQueue(string connection, string name)
            : this(new BusManagementClient(connection), name)
        {
        }

        public InitializeQueue(IBusManagementClient client, string name)
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

        public virtual async Task<bool> CreateIfNotExists()
        {
            var exists = await client.QueueExists(name);
            if (!exists)
            {
                await client.QueueCreate(name);
                exists = true;
            }

            return exists;
        }
        public virtual string Name { get {return name;} }

        public virtual async Task Delete()
        {
            await this.client.QueueDelete(name);
        }
    }
}