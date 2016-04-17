namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using Azure.Data;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicSubscriptionClientTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IAzureStorage topic;
        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = string.Format("a{0}b", random.Next());

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

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            var bq = new BusTopicClient(name, connection);
            await bq.Send(msg);
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new BrokeredMessage[] { new BrokeredMessage(), new BrokeredMessage(), new BrokeredMessage(), new BrokeredMessage() };
            var bq = new BusTopicClient(name, connection);
            await bq.Send(msgs);
        }

        [Test]
        public async Task Receive()
        {
            var expected = Guid.NewGuid();
            var msg = new BrokeredMessage(expected);
            var bq = new BusTopicClient(name, connection);
            await bq.Send(msg);

            var r = new BusSubscriptionClient(name, connection, "testing");
            var resultMsg = await r.Recieve(TimeSpan.FromSeconds(10));
            var result = resultMsg.GetBody<Guid>();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task ReceiveBatch()
        {
            var random = new Random();
            var count = random.Next(1, 11);
            var sent = new List<Guid>();
            var bq = new BusTopicClient(name, connection);
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                var msg = new BrokeredMessage(expected);
                await bq.Send(msg);
                sent.Add(expected);
            }

            var r = new BusSubscriptionClient(name, connection, "testing");

            var got = await r.RecieveBatch(count, TimeSpan.FromSeconds(10));
            foreach (var msg in got)
            {
                var result = msg.GetBody<Guid>();
                Assert.IsTrue(sent.Contains(result));
            }
        }
    }
}