﻿namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusQueueShardsTests
    {
        const string ConnectionString = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            var sqs = new BusQueueShardSender("test", ConnectionString, 2);
        }

        [Test]
        public void IsIBusShardSender()
        {
            Assert.IsNotNull(new BusQueueShardSender("test", ConnectionString, 2) as IBusShardSender);
        }

        [Test]
        public void ShardCount()
        {
            var random = new Random();
            var count = (byte)random.Next(1, 1000);
            var sqs = new BusQueueShardSender("test", ConnectionString, count);
            Assert.AreEqual(count, sqs.ShardCount);
        }

        [Test]
        public void ConstructorConnectionNull()
        {
            Assert.That(() => new BusQueueShardSender("test", null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new BusQueueShardSender(null, ConnectionString), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorShardDefault()
        {
            var sqs = new BusQueueShardSender("test", ConnectionString);
            Assert.AreEqual(2, sqs.ShardCount);
        }

        [Test]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new BusQueueShardSender("test", ConnectionString) as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var sqs = new BusQueueShardSender(name, ConnectionString, 2);
            Assert.AreEqual(name, sqs.Name);
        }

        [Test]
        public async Task CreateIfNotExists()
        {
            var random = new Random();
            var i = random.Next(1, byte.MaxValue);
            var qs = new List<IBusShard>();
            for (var j = 0; j < i; j++)
            {
                var q = Substitute.For<IBusShard>();
                var r = Substitute.For<IAzureStorage>();
                r.CreateIfNotExists().Returns(Task.FromResult(true));
                q.Resource.Returns(r);
                qs.Add(q);
            }
            var sqs = new BusQueueShardSender(qs.ToArray());

            var success = await sqs.CreateIfNotExists();
            Assert.IsTrue(success);

            foreach (var q in qs)
            {
                await q.Resource.Received().CreateIfNotExists();
            }
        }

        [Test]
        public async Task Delete()
        {
            var random = new Random();
            var i = random.Next(1, byte.MaxValue);
            var qs = new List<IBusShard>();
            for (var j = 0; j < i; j++)
            {
                var q = Substitute.For<IBusShard>();
                var r = Substitute.For<IAzureStorage>();
                r.Delete().Returns(Task.FromResult(true));
                q.Resource.Returns(r);
                qs.Add(q);
            }
            var sqs = new BusQueueShardSender(qs.ToArray());

            await sqs.Delete();

            foreach (var q in qs)
            {
                await q.Resource.Received().Delete();
            }
        }

        [Test]
        public async Task Save()
        {
            var random = new Random();
            var i = (byte)random.Next(1, byte.MaxValue);
            var index = random.Next(0, i);

            var msg = new object();
            var qs = new List<IBusShard>();
            for (var j = 0; j < i; j++)
            {
                var q = Substitute.For<IBusShard>();
                var s = Substitute.For<IBusMessageSender>();
                s.Send(msg).Returns(Task.CompletedTask);
                q.Sender.Returns(s);
                qs.Add(q);
            }

            var sqs = new BusQueueShardSender(qs);

            await sqs.Save(msg, (byte)index);

            for (var j = 0; j < i; j++)
            {
                if (j == index)
                {
                    await qs[j].Sender.Received().Send(msg);
                }
                else
                {
                    await qs[j].Sender.DidNotReceive().Send(msg);
                }
            }
        }

        [Test]
        public void Index()
        {
            var msg = new object();
            var q = Substitute.For<IBusShard>();

            var qs = new List<IBusShard>();
            qs.Add(q);
            qs.Add(q);
            qs.Add(q);

            var sqs = new BusQueueShardSender(qs);

            var index = sqs.Index(0);

            Assert.IsTrue(0 <= index && 3 > index);
        }

        [Test]
        public void IndexBad([Values(0, 255)] int val, [Values(0, 0)] int expected)
        {
            var msg = new object();
            var q = Substitute.For<IBusShard>();

            var qs = new List<IBusShard>();
            qs.Add(q);

            var sqs = new BusQueueShardSender(qs);

            var index = sqs.Index((byte)val);

            Assert.AreEqual(expected, index);
        }
    }
}