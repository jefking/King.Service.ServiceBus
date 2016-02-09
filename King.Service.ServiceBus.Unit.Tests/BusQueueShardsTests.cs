namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueShardsTests
    {
        private const string ConnectionString = "UseDevelopmentStorage=true;";

        [Test]
        public void Constructor()
        {
            var sqs = new BusQueueShards("test", ConnectionString, 2);
            Assert.AreEqual(2, sqs.Queues.Count());
        }

        [Test]
        public void ConstructorConnectionNull()
        {
            Assert.That(() => new BusQueueShards("test", null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new BusQueueShards(null, ConnectionString), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorQueuesNull()
        {
            Assert.That(() => new BusQueueShards(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorQueuesEmpty()
        {
            Assert.That(() => new BusQueueShards(new IBusQueueSender[0]), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorShardDefault()
        {
            var sqs = new BusQueueShards("test", ConnectionString);
            Assert.AreEqual(2, sqs.Queues.Count());
        }

        [Test]
        public void IsIQueueShardSender()
        {
            Assert.IsNotNull(new BusQueueShards("test", ConnectionString) as IQueueShardSender<IBusQueueSender>);
        }

        [Test]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new BusQueueShards("test", ConnectionString) as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var sqs = new BusQueueShards(name, ConnectionString, 2);
            Assert.AreEqual(name, sqs.Name);
        }

        [Test]
        public void Queues()
        {
            var random = new Random();
            var i = (byte)random.Next(1, byte.MaxValue);
            var sqs = new BusQueueShards("test", ConnectionString, i);
            Assert.IsNotNull(sqs.Queues);
            Assert.AreEqual(i, sqs.Queues.Count());
        }

        [Test]
        public async Task CreateIfNotExists()
        {
            var random = new Random();
            var i = random.Next(1, byte.MaxValue);
            var qs = new List<IBusQueueSender>();
            for (var j = 0; j < i; j++)
            {
                var q = Substitute.For<IBusQueueSender>();
                q.CreateIfNotExists().Returns(Task.FromResult(true));
                qs.Add(q);
            }
            var sqs = new BusQueueShards(qs.ToArray());

            var success = await sqs.CreateIfNotExists();
            Assert.IsTrue(success);

            foreach (var q in qs)
            {
                await q.Received().CreateIfNotExists();
            }
        }

        [Test]
        public async Task Delete()
        {
            var random = new Random();
            var i = random.Next(1, byte.MaxValue);
            var qs = new List<IBusQueueSender>();
            for (var j = 0; j < i; j++)
            {
                var q = Substitute.For<IBusQueueSender>();
                q.Delete().Returns(Task.FromResult(true));
                qs.Add(q);
            }
            var sqs = new BusQueueShards(qs.ToArray());

            await sqs.Delete();

            foreach (var q in qs)
            {
                await q.Received().Delete();
            }
        }

        [Test]
        public async Task Save()
        {
            var random = new Random();
            var i = (byte)random.Next(1, byte.MaxValue);
            var index = random.Next(0, i);

            var msg = new object();
            var qs = new List<IBusQueueSender>();

            for (var j = 0; j < i; j++)
            {
                var q = Substitute.For<IBusQueueSender>();
                q.Send(msg).Returns(Task.CompletedTask);
                qs.Add(q);
            }

            var sqs = new BusQueueShards(qs);

            await sqs.Save(msg, (byte)index);

            for (var j = 0; j < i; j++)
            {
                if (j == index)
                {
                    await qs[j].Received().Send(msg);
                }
                else
                {
                    await qs[j].DidNotReceive().Send(msg);
                }
            }
        }

        [Test]
        public void Index()
        {
            var msg = new object();
            var q = Substitute.For<IBusQueueSender>();

            var qs = new List<IBusQueueSender>();
            qs.Add(q);
            qs.Add(q);
            qs.Add(q);

            var sqs = new BusQueueShards(qs);

            var index = sqs.Index(0);

            Assert.IsTrue(0 <= index && 3 > index);
        }

        [Test]
        public void IndexBad([Values(0, 255)] int val, [Values(0, 0)] int expected)
        {
            var msg = new object();
            var q = Substitute.For<IBusQueueSender>();

            var qs = new List<IBusQueueSender>();
            qs.Add(q);

            var sqs = new BusQueueShards(qs);

            var index = sqs.Index((byte)val);

            Assert.AreEqual(expected, index);
        }
    }
}