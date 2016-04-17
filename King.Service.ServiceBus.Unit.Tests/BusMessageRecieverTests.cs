namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using NSubstitute;
    using NUnit.Framework;
    using ServiceBus.Wrappers;

    [TestFixture]
    public class BusMessageRecieverTests
    {
        [Test]
        public void ServerWaitTime()
        {
            var random = new Random();
            var wait = random.Next(1, 500);
            var reciever = Substitute.For<IBusReciever>();
            var bsr = new BusMessageReciever(reciever, wait);
            Assert.AreEqual(TimeSpan.FromSeconds(wait), bsr.ServerWaitTime);

        }

        [Test]
        public void ServerWaitTimeDefault()
        {
            var reciever = Substitute.For<IBusReciever>();
            var bsr = new BusMessageReciever(reciever);
            Assert.AreEqual(TimeSpan.FromSeconds(BusMessageReciever.DefaultWaitTime), bsr.ServerWaitTime);
        }
    }
}