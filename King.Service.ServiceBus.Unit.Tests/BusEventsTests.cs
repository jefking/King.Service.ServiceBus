namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusEventsTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNameNull()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BusEvents<object>(null, handler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorConnectionStringNull()
        {
            var queue = Substitute.For<IBusQueue>();
            new BusEvents<object>(queue, null);
        }
    }
}