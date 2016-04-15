namespace King.Service.ServiceBus.Integration.Test
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicTests
    {
        private static readonly string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        [Test]
        public async Task Create()
        {
            var random = new Random();
            var name = "a" + random.Next() + "b";
            var init = new BusTopic(name, connection);
            var e = await init.CreateIfNotExists();
            Assert.IsTrue(e);
        }

        [Test]
        public async Task CreateTwice()
        {
            var random = new Random();
            var name = "a" + random.Next() + "b";
            var init = new BusTopic(name, connection);
            var e = await init.CreateIfNotExists();
            Assert.IsTrue(e);
            e = await init.CreateIfNotExists();
            Assert.IsFalse(e);
        }

        [Test]
        public async Task Delete()
        {
            var random = new Random();
            var name = "a" + random.Next() + "b";
            var init = new BusTopic(name, connection);
            var e = await init.CreateIfNotExists();
            Assert.IsTrue(e);
            await init.Delete();
        }

        [Test]
        public async Task DeleteTwice()
        {
            var random = new Random();
            var name = "a" + random.Next() + "b";
            var init = new BusTopic(name, connection);
            var e = await init.CreateIfNotExists();
            Assert.IsTrue(e);
            await init.Delete();
            await init.Delete();
        }
    }
}