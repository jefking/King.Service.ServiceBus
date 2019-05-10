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
    public class InitializeTopicTaskTests
    {
        private string conn = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new InitializeTopicTask("fake", conn);
        }
        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new InitializeTopicTask((string)null, conn), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new InitializeTopicTask((IBusManagementClient)null, conn), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.TopicExists(name).Returns(false);

            var init = new InitializeTopicTask(client, name);
            init.Run();

            client.Received().TopicExists(name);
            client.Received().TopicCreate(name);
        }

        [Test]
        public void CreateExists()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.TopicExists(name).Returns(true);

            var init = new InitializeTopicTask(client, name);
            init.Run();

            client.Received().TopicExists(name);
            client.DidNotReceive().TopicCreate(name);
        }
    }
}