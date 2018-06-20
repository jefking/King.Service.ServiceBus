namespace King.Service.ServiceBus.Test.Integration
{
    using global::Azure.Data.Wrappers;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusTopicTests
    {
        IAzureStorage topic;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());

            topic = new BusTopic(name, Configuration.ConnectionString);
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