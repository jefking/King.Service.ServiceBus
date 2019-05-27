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
    public class InitializeTopicTests
    {
        private string conn = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new InitializeTopic(conn, "fake");
        }
        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new InitializeTopic(conn, (string)null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new InitializeTopic((IBusManagementClient)null, conn), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Create()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.TopicExists(name).Returns(false);

            var init = new InitializeTopic(client, name);
            await init.CreateIfNotExists();

            await client.Received().TopicExists(name);
            await client.Received().TopicCreate(name);
        }

        [Test]
        public async Task CreateExists()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.TopicExists(name).Returns(true);

            var init = new InitializeTopic(client, name);
            await init.CreateIfNotExists();

            await client.Received().TopicExists(name);
            await client.DidNotReceive().TopicCreate(name);
        }
    }
}