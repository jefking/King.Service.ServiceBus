namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using King.Service.ServiceBus.Wrappers;

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
            Func<BrokeredMessage, Task> callback = (BrokeredMessage msg) => { return Task.Run(() => { }); };
            var options = new OnMessageOptions();

            var client = Substitute.For<IBusReciever>();
            client.OnMessage(callback, options);

            var bsr = new BusSubscriptionReciever(client);
            bsr.RegisterForEvents(callback, options);

            client.Received().OnMessage(callback, options);
        }
    }
}