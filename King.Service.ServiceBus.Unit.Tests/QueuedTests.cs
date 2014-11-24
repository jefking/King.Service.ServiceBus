namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Models;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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

        [Test]
        public async Task OnMessageArrived()
        {
            var data = Guid.NewGuid().ToString();
            var msg = new BrokeredMessage(data)
            {
                ContentType = data.GetType().ToString(),
            };

            var queue = new Queued<string>(msg);
            var result = await queue.Data();

            Assert.AreEqual(result, data);
        }
    }
}