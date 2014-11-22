namespace King.Service.ServiceBus.Integration.Test
{
    using NUnit.Framework;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueuedTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IBusQueueSender sender;
        IBusQueueReciever reciever;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            sender = new BusQueueSender(name, connection);
            sender.CreateIfNotExists().Wait();

            reciever = new BusQueueReciever(name, connection);
        }

        [TearDown]
        public void TearDown()
        {
            sender.Delete().Wait();
        }

        [Test]
        public async Task Abandon()
        {
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            var msg = await this.reciever.Get();

            var queued = new Queued<object>(msg);
            await queued.Abandon();
        }

        [Test]
        public async Task Complete()
        {
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            var msg = await this.reciever.Get();

            var queued = new Queued<object>(msg);
            await queued.Complete();
        }
    }
}