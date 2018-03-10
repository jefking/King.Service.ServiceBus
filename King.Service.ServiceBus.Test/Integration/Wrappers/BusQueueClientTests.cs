﻿namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using ServiceBus.Test.Unit;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueClientTests
    {
        private string connection = Configuration.ConnectionString;

        IAzureStorage queue;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            queue = new BusQueue(name, connection);
            queue.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            queue.Delete().Wait();
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            var bq = new BusQueueClient(QueueClient.CreateFromConnectionString(connection, queue.Name));
            await bq.Send(msg);
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new BrokeredMessage[] { new BrokeredMessage(), new BrokeredMessage(), new BrokeredMessage(), new BrokeredMessage() };
            var bq = new BusQueueClient(QueueClient.CreateFromConnectionString(connection, queue.Name));
            await bq.Send(msgs);
        }

        [Test]
        public async Task Receive()
        {
            var expected = Guid.NewGuid();
            var msg = new BrokeredMessage(expected);
            var bq = new BusQueueClient(QueueClient.CreateFromConnectionString(connection, queue.Name));
            await bq.Send(msg);
            var resultMsg = await bq.Recieve(TimeSpan.FromSeconds(10));
            var result = resultMsg.GetBody<Guid>();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task ReceiveBatch()
        {
            var random = new Random();
            var count = random.Next(1, 11);
            var sent = new List<Guid>();
            var bq = new BusQueueClient(QueueClient.CreateFromConnectionString(connection, queue.Name));
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                var msg = new BrokeredMessage(expected);
                await bq.Send(msg);
                sent.Add(expected);
            }

            var got = await bq.RecieveBatch(count, TimeSpan.FromSeconds(10));
            foreach (var msg in got)
            {
                var result = msg.GetBody<Guid>();
                Assert.IsTrue(sent.Contains(result));
            }
        }
    }
}