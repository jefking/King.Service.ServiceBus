namespace King.Service.ServiceBus.Integration.Test
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Models;
    using NUnit.Framework;

    [TestFixture]
    public class QueuedTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IBusQueueSender sender;
        IBusMessageReciever reciever;
        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = string.Format("a{0}b", random.Next());

            var bq = new BusQueue(name, connection);
            bq.CreateIfNotExists().Wait();

            sender = new BusQueueSender(name, connection);

            reciever = new BusQueueReciever(name, connection);
        }

        [TearDown]
        public void TearDown()
        {
            var bq = new BusQueue(name, connection);
            bq.Delete().Wait();
        }

        [Test]
        public async Task Abandon()
        {
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            var msg = await this.reciever.Get(wait);

            var queued = new Queued<object>(msg);
            await queued.Abandon();
        }

        [Test]
        public async Task Complete()
        {
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            var msg = await this.reciever.Get(wait);

            var queued = new Queued<object>(msg);
            await queued.Complete();
        }
    }
}