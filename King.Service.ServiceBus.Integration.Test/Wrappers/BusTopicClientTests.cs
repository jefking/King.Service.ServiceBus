namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusTopicClientTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            this.name = string.Format("a{0}b", random.Next());

            var init = new InitializeTopic(name, connection);
            init.Run();
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            var bq = new BusTopicClient(TopicClient.CreateFromConnectionString(connection, this.name));
            await bq.Send(msg);
        }
    }
}