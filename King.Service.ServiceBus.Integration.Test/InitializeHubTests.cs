namespace King.Service.ServiceBus.Integration.Test
{
    using NUnit.Framework;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.ServiceBus;

    [TestFixture]
    public class InitializeHubTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        private string name;
        private InitializeHub hub;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = "a" + random.Next() + "b";
        }

        [TearDown]
        public void TearDown()
        {
            var manager = NamespaceManager.CreateFromConnectionString(connection);

            manager.DeleteEventHub(name);
        }

        [Test]
        public async Task Create()
        {
            var init = new InitializeHub(name, connection);
            await init.RunAsync();
        }

        [Test]
        public async Task CreateTwice()
        {
            var init = new InitializeHub(name, connection);
            await init.RunAsync();
            await init.RunAsync();
        }
    }
}