namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Wrappers;
    using System;
    using System.Threading.Tasks;

    public class InitializeTopic : IAzureStorage
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string name = null;

        public InitializeTopic(string connection, string name)
            : this(new BusManagementClient(connection), name)
        {
        }

        public InitializeTopic(IBusManagementClient client, string name)
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
            var exists = await client.TopicExists(name);
            if (!exists)
            {
                await client.TopicCreate(name);
                exists = true;
            }

            return exists;
        }

        public virtual string Name { get {return name;} }

        public virtual async Task Delete()
        {
            await this.client.TopicDelete(name);
        }
    }
}