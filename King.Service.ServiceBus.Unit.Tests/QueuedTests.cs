namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class QueuedTests
    {
        [Test]
        public void Constructor()
        {
            var msg = new BrokeredMessage();
            new Queued<object>(msg);

        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorMessageNull()
        {
            new Queued<object>(null);
        }
    }
}