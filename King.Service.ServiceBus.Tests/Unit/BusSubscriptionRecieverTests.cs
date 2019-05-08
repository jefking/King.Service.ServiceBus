namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusSubscriptionRecieverTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new BusSubscriptionReciever(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString());
        }

        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new BusSubscriptionReciever(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsBusMessageReciever()
        {
            Assert.IsNotNull(new BusSubscriptionReciever(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString()) as BusMessageReciever);
        }

        [Test]
        public void RegisterForEvents()
        {
            Func<IMessageSession, Message, CancellationToken, Task> callback = (IMessageSession ms, Message m, CancellationToken ct) => { return Task.Run(() => { }); };
            var options = new SessionHandlerOptions(null);

            var client = Substitute.For<IBusReciever>();
            client.OnMessage(callback, options);

            var bsr = new BusSubscriptionReciever(client);
            bsr.RegisterForEvents(callback, options);

            client.Received().OnMessage(callback, options);
        }
    }
}