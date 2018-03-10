namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using System.Linq;
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusQueueAutoScalerTests
    {
        const string ConnectionString = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();
            new BusQueueAutoScaler<object>(count, connection);
        }

        [Test]
        public void ConstructorThroughputNull()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();
            Assert.That(() => new BusQueueAutoScaler<object>(count, connection, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsQueueAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();
            Assert.IsNotNull(new BusQueueAutoScaler<object>(count, connection) as QueueAutoScaler<IQueueConnection<object>>);
        }

        [Test]
        public void Runs()
        {
            var random = new Random();
            var max = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var min = (byte)random.Next(byte.MinValue, max);
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueSetup<object>
            {
                Name = "test",
                Priority = QueuePriority.Low,
                Processor = () => { return Substitute.For<IProcessor<object>>(); },
            };

            var connection = new QueueConnection<object>()
            {
                ConnectionString = ConnectionString,
                Setup = setup,
            };

            var throughput = Substitute.For<IQueueThroughput>();
            throughput.Frequency(setup.Priority).Returns(new Range<byte>(min, max));

            var s = new BusQueueAutoScaler<object>(count, connection, throughput);
            var runs = s.Runs(connection);

            Assert.IsNotNull(runs);
            Assert.AreEqual(min, runs.MinimumPeriodInSeconds);
            Assert.AreEqual(max, runs.MaximumPeriodInSeconds);

            throughput.Received().Frequency(setup.Priority);
        }

        [Test]
        public void RunsSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();

            var s = new BusQueueAutoScaler<object>(count, connection);
            Assert.That(() => s.Runs(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ScaleUnit()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            setup.Name.Returns(Guid.NewGuid().ToString());
            
            var connection = new QueueConnection<object>()
            {
                Setup = setup,
                ConnectionString = ConnectionString,
            };

            var s = new BusQueueAutoScaler<object>(count, connection);
            var unit = s.ScaleUnit(connection);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }

        [Test]
        public void ScaleUnitSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();

            var s = new BusQueueAutoScaler<object>(count, connection);
            var x = s.ScaleUnit(null);
            Assert.That(() => x.ElementAt(0), Throws.TypeOf<ArgumentNullException>());
        }
    }
}