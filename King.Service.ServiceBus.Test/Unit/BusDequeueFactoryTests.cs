namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using System.Linq;
    using King.Service.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusDequeueFactoryTests
    {
        const string ConnectionString = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new BusDequeueFactory(ConnectionString);
        }

        [Test]
        public void IsDequeueFactory()
        {
            Assert.IsNotNull(new BusDequeueFactory(ConnectionString) as DequeueFactory);
        }

        [Test]
        public void InitializeNameNull()
        {
            var f = new BusDequeueFactory(ConnectionString);
            Assert.That(() => f.Initialize((string)null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void InitializeName()
        {
            var f = new BusDequeueFactory(ConnectionString);
            var task = f.Initialize(Guid.NewGuid().ToString());

            Assert.IsNotNull(task);
            Assert.IsNotNull(task as InitializeStorageTask);
        }

        [Test]
        public void Tasks()
        {
            var setup = new QueueSetup<object>
            {
                Name = "test",
                Priority = QueuePriority.Low,
            };
            var f = new BusDequeueFactory(ConnectionString);
            var tasks = f.Tasks(setup);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());

            var t = (from n in tasks
                     where n.GetType() == typeof(InitializeStorageTask)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }

        [Test]
        public void TasksSetupNull()
        {
            BusDequeueFactory f = new BusDequeueFactory(ConnectionString);
            var x = f.Tasks<object>((IQueueSetup<object>)null);
            Assert.That(() => x.ElementAt(0), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void DequeueTask()
        {
            var setup = new QueueSetup<object>
            {
                Name = "test",
                Priority = QueuePriority.High,
            };

            var random = new Random();
            var max = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var min = (byte)random.Next(byte.MinValue, max);

            var throughput = Substitute.For<IQueueThroughput>();
            throughput.Scale(setup.Priority).Returns(new Range<byte>(min, max));
            throughput.CheckScaleEvery(setup.Priority).Returns<byte>(1);

            var f = new BusDequeueFactory(ConnectionString, throughput);
            var task = f.Dequeue<object>(setup);

            Assert.IsNotNull(task);
            var scaler = task as BusQueueAutoScaler<object>;
            Assert.IsNotNull(scaler);

            throughput.Received().Scale(setup.Priority);
            throughput.Received().CheckScaleEvery(setup.Priority);
        }

        [Test]
        public void DequeueTaskSetupNull()
        {
            var f = new BusDequeueFactory(ConnectionString);
            Assert.That(() => f.Dequeue<object>(null), Throws.TypeOf<ArgumentNullException>());
        }
    }
}