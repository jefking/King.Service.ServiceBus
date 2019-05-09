namespace King.Service.ServiceBus.Integration.Test.Wrappers
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

        [Test]
        public async Task Send()
        {
            var queue = new BusQueueClient("testsend", connection);
            await queue.Send(new Message());
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new Message[] { new Message(), new Message(), new Message(), new Message() };

            var queue = new BusQueueClient("testsendbatch", connection);
            await queue.Send(msgs);
        }

        [Test]
        public async Task Receive()
        {
            var expected = Guid.NewGuid();
            var msg = new Message(expected.ToByteArray());

            var queue = new BusQueueClient("testsendrecieve", connection);
            queue.OnMessage(this.Get, new MessageHandlerOptions(null));

            await queue.Send(msg);

            for (var i = 0; i < 100 && null == message; i++)
            {

            }

            if (null == message)
            {
                Assert.Fail("did not recieve message.");
            }
            else
            {
                var result = new Queued<Guid>(message);

                Assert.AreEqual(expected, result);
            }
        }

        Message message = null;

        private async Task Get(Message m, CancellationToken ct)
        {
            this.message = m;
        }
    }
}