namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class InitializeSubscriptionTests
    {
        private string conn = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new InitializeSubscription(conn, "fake", "none");
        }

        [Test]
        public void ConstructorTopicNull()
        {
            Assert.That(() => new InitializeSubscription(conn, (string)null, "sub"), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorSubNull()
        {
            Assert.That(() => new InitializeSubscription(conn, "topic", (string)null), Throws.TypeOf<ArgumentException>());
        }
        
        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new InitializeSubscription((IBusManagementClient)null, conn, "topic"), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Create()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.SubscriptionExists(topic, sub).Returns(false);

            var init = new InitializeSubscription(client, topic, sub);
            await init.CreateIfNotExists();

            await client.Received().SubscriptionExists(topic, sub);
            await client.Received().SubscriptionCreate(topic, sub);
        }

        [Test]
        public async Task CreateExists()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.SubscriptionExists(topic, sub).Returns(true);

            var init = new InitializeSubscription(client, topic, sub);
            await init.CreateIfNotExists();

            await client.Received().SubscriptionExists(topic, sub);
            await client.DidNotReceive().SubscriptionCreate(topic, sub);
        }
    }
}