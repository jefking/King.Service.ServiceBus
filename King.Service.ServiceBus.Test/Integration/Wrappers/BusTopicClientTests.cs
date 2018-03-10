﻿namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusTopicClientTests
    {
        private string connection = King.Service.ServiceBus.Test.Integration.Configuration.ConnectionString;

        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            this.name = string.Format("a{0}b", random.Next());

            var init = new BusTopic(name, connection);
            init.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            var init = new BusTopic(name, connection);
            init.Delete().Wait();
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            var bq = new BusTopicClient(TopicClient.CreateFromConnectionString(connection, this.name));
            await bq.Send(msg);
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new BrokeredMessage[] { new BrokeredMessage(), new BrokeredMessage(), new BrokeredMessage(), new BrokeredMessage() };

            var bq = new BusTopicClient(TopicClient.CreateFromConnectionString(connection, this.name));
            await bq.Send(msgs);
        }
    }
}