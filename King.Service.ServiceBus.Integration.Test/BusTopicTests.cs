namespace King.Service.ServiceBus.Integration.Test
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using King.Azure.Data;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicTests
    {
        private static readonly string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        IAzureStorage topic;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());

            topic = new BusTopic(name, connection);
        }

        [TearDown]
        public void TearDown()
        {
            topic.Delete().Wait();
        }

        [Test]
        public async Task Create()
        {
            var e = await topic.CreateIfNotExists();
            Assert.IsTrue(e);
        }

        [Test]
        public async Task CreateTwice()
        {
            var e = await topic.CreateIfNotExists();
            Assert.IsTrue(e);
            e = await topic.CreateIfNotExists();
            Assert.IsFalse(e);
        }

        [Test]
        public async Task Delete()
        {
            var e = await topic.CreateIfNotExists();
            Assert.IsTrue(e);
            await topic.Delete();
        }

        [Test]
        public async Task DeleteTwice()
        {
            var e = await topic.CreateIfNotExists();
            Assert.IsTrue(e);
            await topic.Delete();
            await topic.Delete();
        }
    }
}