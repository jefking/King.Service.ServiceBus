namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueuedTests
    {
        [Test]
        public void Constructor()
        {
            var msg = new Message();
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
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                var data = Guid.NewGuid();
                formatter.Serialize(stream, data);
                var msg = new Message(stream.ToArray());

                var queue = new Queued<Guid>(msg);
                var result = await queue.Data();

                Assert.AreEqual(result, data);
            }
        }

        [Test]
        public async Task OnMessageArrivedJson()
        {
            var data = Guid.NewGuid();
            var json = JsonConvert.SerializeObject(data);
            var bytes = System.Text.Encoding.ASCII.GetBytes(json);
            var msg = new Message(bytes)
            {
                ContentType = data.GetType().ToString(),
            };
            msg.UserProperties.Add(BusQueueClient.EncodingKey, Encoding.Json);

            var queue = new Queued<Guid>(msg);
            var result = await queue.Data();

            Assert.AreEqual(result, data);
        }

        [Test]
        public async Task OnMessageArrivedBinary()
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                var data = Guid.NewGuid();
                formatter.Serialize(stream, data);
                var msg = new Message(stream.ToArray())
                {
                    ContentType = data.GetType().ToString(),
                };
                msg.UserProperties.Add(BusQueueClient.EncodingKey, Encoding.Binary);

                var queue = new Queued<Guid>(msg);
                var result = await queue.Data();

                Assert.AreEqual(result, data);
            }
        }
    }
}