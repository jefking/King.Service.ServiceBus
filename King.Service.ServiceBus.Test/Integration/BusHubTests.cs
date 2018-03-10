namespace King.Service.ServiceBus.Test.Integration
{
    using global::Azure.Data.Wrappers;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusHubTests
    {
        IAzureStorage queue;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            queue = new BusQueue(name, Configuration.ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            queue.Delete().Wait();
        }

        [Test]
        public async Task Create()
        {
            var e = await queue.CreateIfNotExists();
            Assert.IsTrue(e);
        }

        [Test]
        public async Task CreateTwice()
        {
            var e = await queue.CreateIfNotExists();
            Assert.IsTrue(e);
            e = await queue.CreateIfNotExists();
            Assert.IsFalse(e);
        }

        [Test]
        public async Task Delete()
        {
            var e = await queue.CreateIfNotExists();
            Assert.IsTrue(e);
            await queue.Delete();
        }

        [Test]
        public async Task DeleteTwice()
        {
            var e = await queue.CreateIfNotExists();
            Assert.IsTrue(e);
            await queue.Delete();
            await queue.Delete();
        }
    }
}