namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ServiceBusQueuePollerTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusQueue>();
            new ServiceBusQueuePoller<object>(queue);

        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            new ServiceBusQueuePoller<object>(null);
        }
    }
}