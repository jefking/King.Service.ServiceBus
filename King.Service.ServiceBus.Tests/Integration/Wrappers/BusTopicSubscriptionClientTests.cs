namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusTopicSubscriptionClientTests
    {
        private string connection = King.Service.ServiceBus.Test.Integration.Configuration.ConnectionString;

        IAzureStorage topic;
        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = string.Format("a{0}b", random.Next());

            //topic = new BusTopic(name, connection);
            //topic.CreateIfNotExists().Wait();

            //var s = new BusTopicSubscription(topic.Name, connection, "testing");
            //s.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            topic.Delete().Wait();
        }

        [Test]
        public void RegisterForEvents()
        {
            var c = new BusSubscriptionClient(new SubscriptionClient(connection, this.topic.Name, "testing"));
            c.OnMessage((IMessageSession ms, Message m, CancellationToken ct) => { return Task.Run(() => { }); }, new SessionHandlerOptions(null));

            Assert.Fail();
        }

        [Test]
        public async Task Send()
        {
            var msg = new Message();
            var bq = new BusTopicClient(name, connection);
            await bq.Send(msg);
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new Message[] { new Message(), new Message(), new Message(), new Message() };
            var bq = new BusTopicClient(name, connection);
            await bq.Send(msgs);
        }

        [Test]
        public async Task Receive()
        {
            var expected = Guid.NewGuid().ToByteArray();
            var msg = new Message(expected);
            var bq = new BusTopicClient(name, connection);
            await bq.Send(msg);

            //var r = new BusSubscriptionClient(name, connection, "testing");
            //var resultMsg = await r.Recieve(TimeSpan.FromSeconds(10));
            //var result = resultMsg.GetBody<Guid>();
            //Assert.AreEqual(expected, result);

            Assert.Fail();
        }

        [Test]
        public async Task ReceiveBatch()
        {
            var random = new Random();
            var count = random.Next(1, 11);
            var sent = new List<byte[]>();
            var bq = new BusTopicClient(name, connection);
            for (var i = 0; i < count; i++)
            {
               var expected = Guid.NewGuid().ToByteArray();
               var msg = new Message(expected);
               await bq.Send(msg);
               sent.Add(expected);
            }

            //var r = new BusSubscriptionClient(name, connection, "testing");

            //var got = await r.RecieveBatch(count, TimeSpan.FromSeconds(10));
            //foreach (var msg in got)
            //{
            //    var result = msg.GetBody<Guid>();
            //    Assert.IsTrue(sent.Contains(result));
            //}

            Assert.Fail();
        }
    }
}