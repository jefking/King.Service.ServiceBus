namespace King.Service.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Azure.Data;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Topic Subscriber
    /// </summary>
    public class TopicSubscriber
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connection">Connection String</param>
        /// <param name="subscriptionName">Subsciption Name</param>
        /// <param name="sqlFilter">SQL Query (null = all)</param>
        public TopicSubscriber(string name, string connection, string subscriptionName, string sqlFilter = null)
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

            //if (string.IsNullOrWhiteSpace(sqlFilter))
            //{
            //    if (!nm.SubscriptionExists(name, subscriptionName))
            //    {
            //        nm.CreateSubscription(name, subscriptionName);
            //    }
            //}
            //else
            //{
            //    if (!nm.SubscriptionExists(name, subscriptionName))
            //    {
            //        nm.CreateSubscription(name, subscriptionName, new SqlFilter(sqlFilter));
            //    }
            //}
        }
        #endregion
    }

    public class TopicPoller<T> : IPoller<T>
    {
        public Task<IQueued<T>> Poll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IQueued<T>>> PollMany(int messageCount = 5)
        {
            throw new NotImplementedException();
        }
    }

}