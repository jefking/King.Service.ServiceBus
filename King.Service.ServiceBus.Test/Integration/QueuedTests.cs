namespace King.Service.ServiceBus.Test.Integration
{
    using King.Service.ServiceBus.Models;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueuedTests
    {
        IBusMessageSender sender;
        IBusMessageReciever reciever;
        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = string.Format("a{0}b", random.Next());

            //var bq = new BusQueue(name, Configuration.ConnectionString);
            //bq.CreateIfNotExists().Wait();

            sender = new BusQueueSender(name, Configuration.ConnectionString);

            reciever = new BusQueueReciever(name, Configuration.ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            //var bq = new BusQueue(name, Configuration.ConnectionString);
            //bq.Delete().Wait();
        }

        [Test]
        public async Task Abandon()
        {
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            //var msg = await this.reciever.Get(wait);

            //var queued = new Queued<object>(msg);
            //await queued.Abandon();

            Assert.Fail();
        }

        [Test]
        public async Task Complete()
        {
            var wait = TimeSpan.FromSeconds(10);
            var expected = Guid.NewGuid();
            await this.sender.Send(expected);

            //var msg = await this.reciever.Get(wait);

            //var queued = new Queued<object>(msg);
            //await queued.Complete();

            Assert.Fail();
        }
    }
}