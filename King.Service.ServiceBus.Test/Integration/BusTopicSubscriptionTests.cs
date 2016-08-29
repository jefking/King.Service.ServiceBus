namespace King.Service.ServiceBus.Test.Integration
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    public class BusTopicSubscriptionTests
    {
        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            name = string.Format("a{0}b", random.Next());

            var t = new BusTopic(name, Configuration.ConnectionString);
            t.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            var t = new BusTopic(name, Configuration.ConnectionString);
            t.Delete().Wait();
        }

        [Test]
        public async Task CreateIfNotExists()
        {
            var s = new BusTopicSubscription(name, Configuration.ConnectionString, "subsciption");
            var c = await s.CreateIfNotExists();

            Assert.IsTrue(c);
        }

        [Test]
        public async Task CreateIfNotExistsWithFilter()
        {
            var s = new BusTopicSubscription(name, Configuration.ConnectionString, "subsciption", "MesageId > 100");
            var c = await s.CreateIfNotExists();

            Assert.IsTrue(c);
        }

        [Test]
        public async Task Delete()
        {
            var s = new BusTopicSubscription(name, Configuration.ConnectionString, "subsciption");
            var c = await s.CreateIfNotExists();

            await s.Delete();
        }
    }
}