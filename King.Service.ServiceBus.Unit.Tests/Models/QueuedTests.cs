namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Models;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using ServiceBus.Wrappers;
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
        public void ConstructorMessageNull()
        {
            Assert.That(() => new Queued<object>(null), Throws.TypeOf<ArgumentNullException>());
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

        [Test]
        public async Task OnMessageArrivedJson()
        {
            var data = Guid.NewGuid().ToString();
            var json = JsonConvert.SerializeObject(data);
            var msg = new BrokeredMessage(json)
            {
                ContentType = data.GetType().ToString(),
            };
            msg.Properties.Add(BusQueueClient.EncodingKey, Encoding.Json);

            var queue = new Queued<Guid>(msg);
            var result = await queue.Data();

            Assert.AreEqual(result, data);
        }

        [Test]
        public async Task OnMessageArrivedBinary()
        {
            var data = Guid.NewGuid().ToString();
            var msg = new BrokeredMessage(data)
            {
                ContentType = data.GetType().ToString(),
            };
            msg.Properties.Add(BusQueueClient.EncodingKey, Encoding.Binary);

            var queue = new Queued<Guid>(msg);
            var result = await queue.Data();

            Assert.AreEqual(result, data);
        }
    }
}