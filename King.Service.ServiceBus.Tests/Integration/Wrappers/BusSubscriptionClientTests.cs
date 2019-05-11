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
    public class BusSubscriptionClientTests
    {
        private static readonly string connection = King.Service.ServiceBus.Test.Integration.Configuration.ConnectionString;

        private string recieveName, subName;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            recieveName = string.Format("a{0}b", random.Next());
            subName = "sub34";

            var client = new BusManagementClient(connection);
            Task.WaitAll(
                new Task[] {
                    client.TopicCreate(recieveName)
                    , client.SubscriptionCreate(recieveName, subName)
                }
            );
        }

        [TearDown]
        public void TearDown()
        {
            var client = new BusManagementClient(connection).Client;
            Task.WaitAll(
                new Task[] {
                    client.DeleteTopicAsync(recieveName)
                }
            );
        }

        [Test]
        public async Task Receive()
        {
            var expected = Guid.NewGuid().ToByteArray();
            var msg = new Message(expected);

            var rClient = new BusSubscriptionClient(connection, recieveName, subName);
            rClient.OnMessage(this.Get, new MessageHandlerOptions( async (ExceptionReceivedEventArgs args) => { await Task.Run(() => {});}));

            var sClient = new BusTopicClient(connection, recieveName);
            await sClient.Send(msg);

            for (var i = 0; i < 100 && null == message; i++)
            {
                Thread.Sleep(25);
            }

            if (null == message)
            {
                Assert.Fail("did not recieve message.");
            }
            else
            {
                Assert.AreEqual(expected, message.Body);
            }
        }

        Message message = null;

        private async Task Get(Message m, CancellationToken ct)
        {
            this.message = m;
        }
    }
}