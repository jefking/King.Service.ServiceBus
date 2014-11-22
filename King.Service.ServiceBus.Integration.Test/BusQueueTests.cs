namespace King.Service.ServiceBus.Integration.Test
{
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IBusQueueSender sender;
        IBusQueueReciever reciever;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            sender = new BusQueueSender(name, connection);
            sender.CreateIfNotExists().Wait();

            reciever = new BusQueueReciever(name, connection);
        }

        [TearDown]
        public void TearDown()
        {
            sender.Delete().Wait();
        }

        [Test]
        public async Task CreateIfNotExists()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var q = new BusQueue(name, connection);
            var result = await q.CreateIfNotExists();
            Assert.IsTrue(result);
            result = await q.CreateIfNotExists();
            Assert.IsFalse(result);

            await q.Delete();
        }
        [Test]
        public async Task Delete()
        {
            await sender.Delete();
            var result = await sender.CreateIfNotExists();
            Assert.IsTrue(result);

            await sender.Delete();
            result = await sender.CreateIfNotExists();
            Assert.IsTrue(result);

            await sender.Delete();
        }

        [Test]
        public async Task ApproixmateMessageCount()
        {
            var msg = new BrokeredMessage();
            await this.sender.Send(msg);
            var count = await this.sender.ApproixmateMessageCount();
            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task SendBrokeredMessage()
        {
            var msg = new BrokeredMessage();
            await this.sender.Send(msg);
        }

        [Test]
        public async Task SendBrokeredMessageAsObject()
        {
            var msg = new BrokeredMessage();
            await this.sender.Send((object)msg);
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            await this.sender.Send(Guid.NewGuid());
        }

        [Test]
        public async Task Get()
        {
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            var msg = await this.reciever.Get();
            var result = msg.GetBody<Guid>();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetMany()
        {
            var random = new Random();
            var count = random.Next(1, 11);
            var sent = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                await this.sender.Send(expected);
                sent.Add(expected);
            }

            var got = await this.reciever.GetMany(count);
            foreach (var msg in got)
            {
                var result = msg.GetBody<Guid>();
                Assert.IsTrue(sent.Contains(result));
            }
        }

        [Test]
        public async Task GetManyNegative()
        {
            var random = new Random();
            var count = random.Next(5, 11);
            var sent = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                await this.sender.Send(expected);
                sent.Add(expected);
            }

            var got = await this.reciever.GetMany(-count);
            Assert.AreEqual(5, got.Count());
            foreach (var msg in got)
            {
                var result = msg.GetBody<Guid>();
                Assert.IsTrue(sent.Contains(result));
            }
        }

        [Test]
        public async Task RecieveEvent()
        {
            this.reciever.RegisterForEvents(OnMessageArrived, new OnMessageOptions());
            this.sender.Send(Guid.NewGuid());
            var ii = 10;
            while (this.eventCount == 0 && ii > 0)
            {
                Thread.Sleep(500);
                ii--;
            }

            Assert.AreEqual(1, this.eventCount);
        }

        int eventCount = 0;

        private async Task OnMessageArrived(BrokeredMessage message)
        {
            eventCount++;
            await new TaskFactory().StartNew(() => { });
        }
    }
}