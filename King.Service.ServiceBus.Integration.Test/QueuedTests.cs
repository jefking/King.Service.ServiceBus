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

        IBusQueue queue;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            queue = new BusQueue(name, connection);
            queue.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            queue.Delete().Wait();
        }

        [Test]
        public async Task Abandon()
        {
            var expected = Guid.NewGuid();
            await this.queue.Send(expected);

            var msg = await this.queue.Get();

            var queued = new Queued<object>(msg);
            await queued.Abandon();
        }

        [Test]
        public async Task Complete()
        {
            var expected = Guid.NewGuid();
            await this.queue.Send(expected);

            var msg = await this.queue.Get();

            var queued = new Queued<object>(msg);
            await queued.Complete();
        }
    }
}