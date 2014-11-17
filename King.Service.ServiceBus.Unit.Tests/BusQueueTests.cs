namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusQueueTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNull()
        {
            new BusQueue(null, "asd");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new BusQueue(Guid.NewGuid().ToString(), null);
        }
    }
}