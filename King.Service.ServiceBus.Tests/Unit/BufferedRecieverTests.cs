namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BufferedRecieverTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BufferedReciever<object>(queue, handler);
        }

        [Test]
        public void IsBusEventsBufferedMessage()
        {
            var queue = Substitute.For<IBusReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            Assert.IsNotNull(new BufferedReciever<object>(queue, handler) as BusEvents<BufferedMessage>);
        }

        [Test]
        public void DefaultConcurrentCalls()
        {
            Assert.AreEqual(50, BufferedReciever<object>.DefaultConcurrentCalls);
        }
    }
}