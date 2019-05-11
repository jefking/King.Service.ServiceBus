namespace King.Service.ServiceBus.Test.Integration.Wrappers
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusTopicClientTests
    {
        private static readonly string connection = King.Service.ServiceBus.Test.Integration.Configuration.ConnectionString;

        private string sendName, recieveName, sendBatchName;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            sendName = string.Format("a{0}b", random.Next());
            sendBatchName = string.Format("a{0}b", random.Next());

            var client = new BusManagementClient(connection);
            Task.WaitAll(
                new Task[] {
                    client.TopicCreate(sendName)
                    , client.TopicCreate(sendBatchName)
                }
            );
        }

        [TearDown]
        public void TearDown()
        {
            var client = new BusManagementClient(connection).Client;
            Task.WaitAll(
                new Task[] {
                    client.DeleteTopicAsync(sendName)
                    , client.DeleteTopicAsync(sendBatchName)
                }
            );
        }

        [Test]
        public async Task Send()
        {
            var queue = new BusTopicClient(sendName, connection);
            await queue.Send(new Message());
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new Message[] { new Message(), new Message(), new Message(), new Message() };

            var queue = new BusTopicClient(sendBatchName, connection);
            await queue.Send(msgs);
        }
    }
}