﻿namespace King.Service.ServiceBus.Test.Unit
{
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using King.Service.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusDequeueTests
    {
        [Test]
        public void Constructor()
        {
            var processor = Substitute.For<IProcessor<object>>();
            var queue = Substitute.For<IBusMessageReciever>();
            new BusDequeue<object>(queue, processor);
        }

        [Test]
        public void IsDequeue()
        {
            var processor = Substitute.For<IProcessor<object>>();
            var queue = Substitute.For<IBusMessageReciever>();
            Assert.IsNotNull(new BusDequeue<object>(queue, processor) as Dequeue<object>);
        }
    }
}