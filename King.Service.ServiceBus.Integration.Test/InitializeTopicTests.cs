namespace King.Service.ServiceBus.Integration.Test
{
    using NUnit.Framework;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

    [TestFixture]
    public class InitializeTopicTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        [Test]
        public async Task Create()
        {
            var random = new Random();
            var name = "a" + random.Next() + "b";
            var init = new InitializeTopic(name, connection);
            await init.RunAsync();
        }

        [Test]
        public async Task CreateTwice()
        {
            var random = new Random();
            var name = "a" + random.Next() + "b";
            var init = new InitializeTopic(name, connection);
            await init.RunAsync();
            await init.RunAsync();
        }
    }
}