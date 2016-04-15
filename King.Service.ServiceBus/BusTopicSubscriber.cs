namespace King.Service.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    /// <summary>
    /// Topic Subscriber
    /// </summary>
    public class BusTopicSubscriber
    {
        #region Members
        /// <summary>
        /// Subscription Description
        /// </summary>
        protected readonly SubscriptionDescription desciption;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connection">Connection String</param>
        /// <param name="subscriptionName">Subsciption Name</param>
        /// <param name="sqlFilter">SQL Query (null = all)</param>
        public BusTopicSubscriber(string name, string connection, string subscriptionName, string sqlFilter = null)
        {
            if (string.IsNullOrWhiteSpace(name))
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

            var nm = NamespaceManager.CreateFromConnectionString(connection);

            this.desciption = this.Bind(nm, name, subscriptionName, sqlFilter).Result;
        }
        #endregion

        #region Methods
        public virtual async Task<SubscriptionDescription> Bind(NamespaceManager nm, string name, string subscriptionName, string sqlFilter)
        {

            if (!await nm.SubscriptionExistsAsync(name, subscriptionName))
            {
                return (string.IsNullOrWhiteSpace(sqlFilter)) ?
                    await nm.CreateSubscriptionAsync(name, subscriptionName) :
                    await nm.CreateSubscriptionAsync(name, subscriptionName, new SqlFilter(sqlFilter));
            }
            else
            {
                return await nm.GetSubscriptionAsync(name, subscriptionName);
            }
        }
        #endregion
    }
}