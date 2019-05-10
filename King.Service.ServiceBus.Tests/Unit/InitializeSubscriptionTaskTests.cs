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
    public class InitializeSubscriptionTaskTests
    {
        private string conn = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new InitializeSubscriptionTask("fake", "none", conn);
        }

        [Test]
        public void ConstructorTopicNull()
        {
            Assert.That(() => new InitializeSubscriptionTask((string)null, "sub", conn), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorSubNull()
        {
            Assert.That(() => new InitializeSubscriptionTask("topic", (string)null, conn), Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new InitializeQueueTask((IBusManagementClient)null, conn), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.SubscriptionExists(topic, sub).Returns(false);

            var init = new InitializeSubscriptionTask(client, topic, sub);
            init.Run();

            client.Received().SubscriptionExists(topic, sub);
            client.Received().SubscriptionCreate(topic, sub);
        }

        [Test]
        public void CreateExists()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.SubscriptionExists(topic, sub).Returns(true);

            var init = new InitializeSubscriptionTask(client, topic, sub);
            init.Run();

            client.Received().SubscriptionExists(topic, sub);
            client.DidNotReceive().SubscriptionCreate(topic, sub);
        }
    }
}