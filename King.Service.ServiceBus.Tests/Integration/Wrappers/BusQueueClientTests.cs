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
    public class BusQueueClientTests
    {
        private static readonly string connection = King.Service.ServiceBus.Test.Integration.Configuration.ConnectionString;

        private string sendName, recieveName, sendBatchName;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            sendName = string.Format("a{0}b", random.Next());
            recieveName = string.Format("a{0}b", random.Next());
            sendBatchName = string.Format("a{0}b", random.Next());

            var client = new BusManagementClient(connection);
            Task.WaitAll(
                new Task[] {
                    client.QueueCreate(sendName)
                    , client.QueueCreate(recieveName)
                    , client.QueueCreate(sendBatchName)
                }
            );
        }

        [TearDown]
        public void TearDown()
        {
            var client = new BusManagementClient(connection).Client;
            Task.WaitAll(
                new Task[] {
                    client.DeleteQueueAsync(sendName)
                    , client.DeleteQueueAsync(recieveName)
                    , client.DeleteQueueAsync(sendBatchName)
                }
            );
        }

        [Test]
        public async Task Send()
        {
            var queue = new BusQueueClient(sendName, connection);
            await queue.Send(new Message());
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new Message[] { new Message(), new Message(), new Message(), new Message() };

            var queue = new BusQueueClient(sendBatchName, connection);
            await queue.Send(msgs);
        }

        [Test]
        public async Task Receive()
        {
            var expected = Guid.NewGuid().ToByteArray();
            var msg = new Message(expected);

            var queue = new BusQueueClient(recieveName, connection);
            queue.OnMessage(this.Get, new MessageHandlerOptions( async (ExceptionReceivedEventArgs args) => { await Task.Run(() => {});}));

            await queue.Send(msg);

            for (var i = 0; i < 100 && null == message; i++)
            {
                Thread.Sleep(20);
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