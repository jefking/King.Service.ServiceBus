namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

    [TestFixture]
    public class HubClientTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            this.name = string.Format("a{0}b", random.Next());

            var init = new BusHub(name, connection);
            init.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            var init = new BusHub(name, connection);
            init.Delete().Wait();
        }

        [Test]
        public async Task Send()
        {
            var msg = new EventData(Guid.NewGuid().ToByteArray());
            var bq = new HubClient(EventHubClient.CreateFromConnectionString(connection, this.name));
            await bq.Send(msg);
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new EventData[] { new EventData(Guid.NewGuid().ToByteArray()), new EventData(Guid.NewGuid().ToByteArray()), new EventData(Guid.NewGuid().ToByteArray()), new EventData(Guid.NewGuid().ToByteArray()) };

            var bq = new HubClient(EventHubClient.CreateFromConnectionString(connection, this.name));
            await bq.Send(msgs);
        }
    }
}