namespace King.Service.ServiceBus.Integration.Test
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using NUnit.Framework;

    public class BusTopicSubscriberTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
        }

        [TearDown]
        public void TearDown()
        {
            var s = new BusTopicSubscriber(name, connection, "subsciption");
            s.Delete().Wait();
        }

        [Test]
        public async Task CreateIfNotExists()
        {
            var s = new BusTopicSubscriber(name, connection, "subsciption");
            var c = await s.CreateIfNotExists();

            Assert.IsTrue(c);
        }

        [Test]
        public async Task Delete()
        {
            var s = new BusTopicSubscriber(name, connection, "subsciption");
            var c = await s.CreateIfNotExists();

            Assert.IsTrue(c);

            await s.Delete();
        }
    }
}