namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using King.Service;
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class InitializeRuleTask : InitializeTask
    {
        #region Members
        protected readonly IBusManagementClient client = null;
        protected readonly string topicPath = null
                                , subscriptionName = null
                                , name = null;
        protected readonly Filter filter = null;
        protected readonly bool deleteDefault;
        #endregion

        #region Constructors
        public InitializeRuleTask(string connection, string topicPath, string subscriptionName, string name, Filter filter, bool deleteDefault = true)
            : this(new BusManagementClient(connection), topicPath, subscriptionName, name, filter, deleteDefault)
        {
        }

        public InitializeRuleTask(IBusManagementClient client, string topicPath, string subscriptionName, string name, Filter filter, bool deleteDefault = true)
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
            this.deleteDefault = deleteDefault;
        }
        #endregion

        public override async Task RunAsync()
        {   
            if (deleteDefault)
            {
                try
                {
                    await client.RuleGet(topicPath, subscriptionName, RuleDescription.DefaultRuleName);
                    await client.RuleDelete(topicPath, subscriptionName, RuleDescription.DefaultRuleName);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Default rule for {0}:{1} already pruned. {2}", topicPath, name, ex.Message);
                }
            }

            await client.RuleCreate(topicPath, subscriptionName, name, filter);
        }
    }
}