namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public class InitializeRuleTask : InitializeTask
    {
        #region Members
        protected readonly IBusManagementClient client = null;
        protected readonly string topicPath = null
                                , subscriptionName = null
                                , name = null;
        protected readonly Filter filter = null;
        #endregion

        #region Constructors
        public InitializeRuleTask(string connection, string topicPath, string subscriptionName, string name, Filter filter)
            : this(new BusManagementClient(connection), topicPath, subscriptionName, name, filter)
        {
        }

        public InitializeRuleTask(IBusManagementClient client, string topicPath, string subscriptionName, string name, Filter filter)
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
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }
            if (null == filter)
            {
                throw new ArgumentNullException("filter");
            }

            this.client = client;
            this.topicPath = topicPath;
            this.subscriptionName = subscriptionName;
            this.name = name;
            this.filter = filter;
        }
        #endregion

        public override async Task RunAsync()
        {
            await client.CreateRuleAsync(topicPath, subscriptionName, name, filter);
        }
    }
}