namespace King.Service.ServiceBus.Integration.Test
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class BusQueueTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IBusMessageSender sender;
        IBusMessageReciever reciever;
        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = string.Format("a{0}b", random.Next());

            var bq = new BusQueue(name, connection);
            bq.CreateIfNotExists().Wait();

            sender = new BusQueueSender(name, connection);

            reciever = new BusQueueReciever(name, connection);
        }

        [TearDown]
        public void TearDown()
        {
            var bq = new BusQueue(name, connection);
            bq.Delete().Wait();
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
            var bq = new BusQueue(name, connection);

            await bq.Delete();
            var result = await bq.CreateIfNotExists();
            Assert.IsTrue(result);

            await bq.Delete();
            result = await bq.CreateIfNotExists();
            Assert.IsTrue(result);

            await bq.Delete();
        }

        [Test]
        public async Task ApproixmateMessageCount()
        {
            var bq = new BusQueue(name, connection);

            var msg = new BrokeredMessage();
            await this.sender.Send(msg);
            var count = await bq.ApproixmateMessageCount();
            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task LockDuration()
        {
            var bq = new BusQueue(name, connection);

            var lockDuration = await bq.LockDuration();
            Assert.AreEqual(TimeSpan.FromMinutes(1), lockDuration);
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
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected, Encoding.Binary);

            var msg = await this.reciever.Get(wait);
            var result = msg.GetBody<Guid>();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetJson()
        {
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            var msg = await this.reciever.Get(wait);
            var json = msg.GetBody<string>();
            var result = JsonConvert.DeserializeObject<Guid>(json);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetAt()
        {
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected, DateTime.UtcNow.AddMinutes(-1), Encoding.Binary);

            var msg = await this.reciever.Get(wait);
            var result = msg.GetBody<Guid>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetAtJson()
        {
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected, DateTime.UtcNow.AddMinutes(-1));

            var msg = await this.reciever.Get(wait);
            var json = msg.GetBody<string>();
            var result = JsonConvert.DeserializeObject<Guid>(json);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetMany()
        {
            var wait = TimeSpan.FromSeconds(10);
            var random = new Random();
            var count = random.Next(1, 11);
            var sent = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                await this.sender.Send(expected, Encoding.Binary);
                sent.Add(expected);
            }

            var got = await this.reciever.GetMany(wait, count);
            foreach (var msg in got)
            {
                var result = msg.GetBody<Guid>();
                Assert.IsTrue(sent.Contains(result));
            }
        }

        [Test]
        public async Task GetManyJson()
        {
            var wait = TimeSpan.FromSeconds(10);
            var random = new Random();
            var count = random.Next(1, 11);
            var sent = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                await this.sender.Send(expected);
                sent.Add(expected);
            }

            var got = await this.reciever.GetMany(wait, count);
            foreach (var msg in got)
            {
                var json = msg.GetBody<string>();
                var result = JsonConvert.DeserializeObject<Guid>(json);
                Assert.IsTrue(sent.Contains(result));
            }
        }

        [Test]
        public async Task GetManyNegative()
        {
            var wait = TimeSpan.FromSeconds(10);
            var random = new Random();
            var count = random.Next(5, 11);
            var sent = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                await this.sender.Send(expected, Encoding.Binary);
                sent.Add(expected);
            }

            var got = await this.reciever.GetMany(wait, -count);
            Assert.AreEqual(1, got.Count());
            foreach (var msg in got)
            {
                var result = msg.GetBody<Guid>();
                Assert.IsTrue(sent.Contains(result));
            }
        }

        [Test]
        public async Task GetManyNegativeJson()
        {
            var wait = TimeSpan.FromSeconds(10);
            var random = new Random();
            var count = random.Next(5, 11);
            var sent = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var expected = Guid.NewGuid();
                await this.sender.Send(expected);
                sent.Add(expected);
            }

            var got = await this.reciever.GetMany(wait, -count);
            Assert.AreEqual(1, got.Count());
            foreach (var msg in got)
            {
                var json = msg.GetBody<string>();
                var result = JsonConvert.DeserializeObject<Guid>(json);
                Assert.IsTrue(sent.Contains(result));
            }
        }

        [Test]
        public void RecieveEvent()
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