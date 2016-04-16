namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;

    public class BusTopicSubscriptionClientTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        [Test]
        public void RegisterForEvents()
        {
            var name = Guid.NewGuid().ToString();
            var topicPath = Guid.NewGuid().ToString();
            var c = new BusTopicSubscriptionClient(SubscriptionClient.CreateFromConnectionString(connection, topicPath, name));
            c.RegisterForEvents((BrokeredMessage msg) => { return Task.Run(() => { }); }, new OnMessageOptions());
        }
    }
}