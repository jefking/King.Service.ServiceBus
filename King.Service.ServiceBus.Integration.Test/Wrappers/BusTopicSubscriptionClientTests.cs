namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Azure.Data;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;

    public class BusTopicSubscriptionClientTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IAzureStorage topic;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());

            topic = new BusTopic(name, connection);
            topic.CreateIfNotExists().Wait();

            var s = new BusTopicSubscription(topic.Name, connection, "testing");
            s.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            topic.Delete().Wait();
        }

        [Test]
        public void RegisterForEvents()
        {
            var c = new BusSubscriptionClient(SubscriptionClient.CreateFromConnectionString(connection, this.topic.Name, "testing"));
            c.OnMessage((BrokeredMessage msg) => { return Task.Run(() => { }); }, new OnMessageOptions());
        }
    }
}