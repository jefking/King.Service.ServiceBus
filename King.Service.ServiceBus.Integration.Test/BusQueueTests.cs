namespace King.Service.ServiceBus.Integration.Test
{
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IBusQueue queue;

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
            await queue.Delete();
            var result = await queue.CreateIfNotExists();
            Assert.IsTrue(result);

            await queue.Delete();
            result = await queue.CreateIfNotExists();
            Assert.IsTrue(result);

            await queue.Delete();
        }

        [Test]
        public async Task ApproixmateMessageCount()
        {
            var msg = new BrokeredMessage();
            await this.queue.Send(msg);
            var count = await this.queue.ApproixmateMessageCount();
            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task SendBrokeredMessage()
        {
            var msg = new BrokeredMessage();
            await this.queue.Send(msg);
        }

        [Test]
        public async Task SendBrokeredMessageAsObject()
        {
            var msg = new BrokeredMessage();
            await this.queue.Send((object)msg);
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            await this.queue.Send(Guid.NewGuid());
        }

        [Test]
        public async Task Get()
        {
            var expected = Guid.NewGuid();
            await this.queue.Send(expected);

            var msg = await this.queue.Get();
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
                await this.queue.Send(expected);
                sent.Add(expected);
            }

            var got = await this.queue.GetMany(count);
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
                await this.queue.Send(expected);
                sent.Add(expected);
            }

            var got = await this.queue.GetMany(-count);
            Assert.AreEqual(5, got.Count());
            foreach (var msg in got)
            {
                var result = msg.GetBody<Guid>();
                Assert.IsTrue(sent.Contains(result));
            }
        }

        [Test]
        public async Task RegisterForEvents()
        {
            this.queue.RegisterForEvents(OnMessageArrived, new OnMessageOptions());
        }

        private async Task OnMessageArrived(BrokeredMessage message)
        {
            await new TaskFactory().StartNew(() => { });
        }
    }
}