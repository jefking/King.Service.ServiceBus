namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;

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
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new BusDequeueFactory(ConnectionString) as ITaskFactory<IQueueSetup<object>>);
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
                     where n.GetType() == typeof(InitializeBusQueue)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TasksSetupNull()
        {
            var f = new BusDequeueFactory(ConnectionString);
            var tasks = f.Tasks<object>(null);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());
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
            throughput.MaximumScale(setup.Priority).Returns(max);
            throughput.MinimumScale(setup.Priority).Returns(min);

            var f = new BusDequeueFactory(ConnectionString, throughput);
            var task = f.Dequeue<object>(setup);

            Assert.IsNotNull(task);
            var scaler = task as BusQueueAutoScaler<object>;
            Assert.IsNotNull(scaler);
            Assert.AreEqual(min, scaler.Minimum);
            Assert.AreEqual(max, scaler.Maximum);

            throughput.Received().MaximumScale(setup.Priority);
            throughput.Received().MinimumScale(setup.Priority);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DequeueTaskSetupNull()
        {
            var f = new BusDequeueFactory(ConnectionString);
            var task = f.Dequeue<object>(null);
        }
    }
}