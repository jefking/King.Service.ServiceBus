namespace King.Service.ServiceBus.Integration.Test
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using NUnit.Framework;

    public class BusTopicSubscriptionTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = string.Format("a{0}b", random.Next());

            var t = new BusTopic(name, connection);
            t.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            var t = new BusTopic(name, connection);
            t.Delete().Wait();
        }

        [Test]
        public async Task CreateIfNotExists()
        {
            var s = new BusTopicSubscription(name, connection, "subsciption");
            var c = await s.CreateIfNotExists();

            Assert.IsTrue(c);
        }

        [Test]
        public async Task Delete()
        {
            var s = new BusTopicSubscription(name, connection, "subsciption");
            var c = await s.CreateIfNotExists();

            await s.Delete();
        }
    }
}