namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Configuration;

    [TestFixture]
    public class BusDequeueBatchDynamicTests
    {
        static readonly string ConnectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new BusDequeueBatchDynamic<object>(name, ConnectionString, processor);
        }

        [Test]
        public void MockableConstructorSimplified()
        {
            var queue = Substitute.For<IBusQueueReciever>();
            var processor = Substitute.For<IProcessor<object>>();
            new BusDequeueBatchDynamic<object>(queue, processor);
        }

        [Test]
        public void MockableConstructor()
        {
            var queue = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            new BusDequeueBatchDynamic<object>(queue, processor, tracker);
        }

        [Test]
        public void IsDequeueBatchDynamic()
        {
            var queue = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            Assert.IsNotNull(new BusDequeueBatchDynamic<object>(queue, processor, tracker) as DequeueBatchDynamic<object>);
        }
    }
}