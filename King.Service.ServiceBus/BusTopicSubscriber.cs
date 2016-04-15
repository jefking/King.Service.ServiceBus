namespace King.Service.ServiceBus
{
    using Azure.Data;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Topic Subscriber
    /// </summary>
    public class BusTopicSubscriber : IAzureStorage
    {
        #region Members
        /// <summary>
        /// Topic Name
        /// </summary>
        protected readonly string topicName = null;

        /// <summary>
        /// Namespace Manager
        /// </summary>
        protected readonly NamespaceManager manager = null;

        /// <summary>
        /// Subscription Name
        /// </summary>
        protected readonly string subscriptionName;

        /// <summary>
        /// SQL Filter
        /// </summary>
        protected readonly string sqlFilter;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="topicName">Topic Name</param>
        /// <param name="connection">Connection String</param>
        /// <param name="subscriptionName">Subsciption Name</param>
        /// <param name="sqlFilter">SQL Query (null = all)</param>
        public BusTopicSubscriber(string topicName, string connection, string subscriptionName, string sqlFilter = null)
        {
            if (string.IsNullOrWhiteSpace(topicName))
            {
                throw new ArgumentException("name");
            }
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new ArgumentException("connection");
            }
            if (string.IsNullOrWhiteSpace(subscriptionName))
            {
                throw new ArgumentException("subscriptionName");
            }

            this.topicName = topicName;
            this.subscriptionName = subscriptionName;
            this.sqlFilter = string.IsNullOrWhiteSpace(sqlFilter) ? null : sqlFilter;

            this.manager = NamespaceManager.CreateFromConnectionString(connection);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Subscription Name
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.subscriptionName;
            }
        }

        /// <summary>
        /// Topic Name
        /// </summary>
        public virtual string TopicName
        {
            get
            {
                return this.topicName;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns>Created</returns>
        public virtual async Task<bool> CreateIfNotExists()
        {
            var created = false;
            if (!await this.manager.SubscriptionExistsAsync(topicName, subscriptionName))
            {
                var d = (string.IsNullOrWhiteSpace(sqlFilter)) ?
                    await this.manager.CreateSubscriptionAsync(topicName, subscriptionName) :
                    await this.manager.CreateSubscriptionAsync(topicName, subscriptionName, new SqlFilter(sqlFilter));

                created = d.Status == EntityStatus.Creating;
            }

            return created;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <returns>Task</returns>
        public async Task Delete()
        {
            if (!await this.manager.SubscriptionExistsAsync(topicName, subscriptionName))
            {
                await this.manager.DeleteSubscriptionAsync(this.topicName, this.topicName);
            }
        }
        #endregion
    }
}