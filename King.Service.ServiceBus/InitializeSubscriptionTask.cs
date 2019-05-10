namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using System;
    using System.Threading.Tasks;

    public class InitializeSubscriptionTask : InitializeTask
    {
        protected readonly IBusManagementClient client = null;
        protected readonly string topicPath = null
                                , subscriptionName = null;

        public InitializeSubscriptionTask(string topicPath, string subscriptionName, string connection)
            : this(new BusManagementClient(connection), topicPath, subscriptionName)
        {
        }

        public InitializeSubscriptionTask(IBusManagementClient client, string topicPath, string subscriptionName)
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

        public override async Task RunAsync()
        {
            var exists = await client.SubscriptionExists(topicPath, subscriptionName);
            if (!exists)
            {
                await client.SubscriptionCreate(topicPath, subscriptionName);
            }
        }
    }
}