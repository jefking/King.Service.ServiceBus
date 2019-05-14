namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Management;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Service Bus Management Client
    /// </summary>
    public class BusManagementClient : IBusManagementClient
    {
        #region Members
        protected readonly ManagementClient client = null;
        #endregion

        #region Constructors
        public BusManagementClient(string connection)
            : this(new ManagementClient(connection))
        {
        }
        public BusManagementClient(ManagementClient client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Management Client
        /// </summary>
        public virtual ManagementClient Client
        {
            get
            {
                return this.client;
            }
        }
        #endregion

        #region Methods
        public async Task QueueCreate(string queue)
        {
            var desc = new QueueDescription(queue)
            {

            };

            await this.client.CreateQueueAsync(desc);
        }
        public async Task<bool> QueueExists(string queue)
        {
            return await this.client.QueueExistsAsync(queue);
        }
        public async Task TopicCreate(string topicPath)
        {
            await this.client.CreateTopicAsync(topicPath);
        }
        public async Task<bool> TopicExists(string topicPath)
        {
            return await this.client.TopicExistsAsync(topicPath);
        }
        public async Task SubscriptionCreate(string topicPath, string subscriptionName)
        {
            var desc = new SubscriptionDescription(topicPath, subscriptionName)
            {
            };

            await this.client.CreateSubscriptionAsync(desc);
        }
        public async Task<bool> SubscriptionExists(string topicPath, string subscriptionName)
        {
            return await this.client.SubscriptionExistsAsync(topicPath, subscriptionName);
        }
        public async Task CreateRuleAsync(string topicPath, string subscriptionName, string name, Filter filter)
        {
            var rule = new RuleDescription(name, filter);

            await this.client.CreateRuleAsync(topicPath, subscriptionName, rule);
        }
        #endregion
    }
}