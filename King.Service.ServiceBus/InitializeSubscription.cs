namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using System;
    using System.Threading.Tasks;

    public class InitializeSubscription : IAzureStorage
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string topicPath = null
                                , subscriptionName = null;

        public InitializeSubscription(string connection, string topicPath, string subscriptionName)
            : this(new BusManagementClient(connection), topicPath, subscriptionName)
        {
        }

        public InitializeSubscription(IBusManagementClient client, string topicPath, string subscriptionName)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }
            if (string.IsNullOrEmpty(topicPath))
            {
                throw new ArgumentException("topicPath");
            }
            if (string.IsNullOrEmpty(subscriptionName))
            {
                throw new ArgumentException("subscriptionName");
            }

            this.client = client;
            this.topicPath = topicPath;
            this.subscriptionName = subscriptionName;
        }

        public virtual async Task<bool> CreateIfNotExists()
        {
            var exists = await client.SubscriptionExists(topicPath, subscriptionName);
            if (!exists)
            {
                await client.SubscriptionCreate(topicPath, subscriptionName);
                exists = true;
            }

            return exists;
        }
        public virtual string Name { get {return subscriptionName;} }

        public virtual async Task Delete()
        {
            await this.client.SubscriptionDelete(topicPath, subscriptionName);
        }
    }
}