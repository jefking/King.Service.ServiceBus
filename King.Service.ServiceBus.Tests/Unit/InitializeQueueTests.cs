namespace King.Service.ServiceBus.Test.Unit
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class InitializeQueueTests
    {
        private string conn = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new InitializeQueue(conn, "fake");
        }

        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new InitializeQueue(conn, (string)null), Throws.TypeOf<ArgumentException>());
        }
        
        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new InitializeQueue((IBusManagementClient)null, "fake"), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new InitializeQueue(conn, "fake") as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var i = new InitializeQueue(conn, name);
            Assert.AreEqual(name, i.Name);
        }

        [Test]
        public async Task Create()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.QueueExists(name).Returns(false);

            var init = new InitializeQueue(client, name);
            var e = await init.CreateIfNotExists();

            Assert.IsTrue(e);
            await client.Received().QueueExists(name);
            await client.Received().QueueCreate(name);
        }

        [Test]
        public async Task CreateExists()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.QueueExists(name).Returns(true);

            var init = new InitializeQueue(client, name);
            var e = await init.CreateIfNotExists();

            Assert.IsTrue(e);
            await client.Received().QueueExists(name);
            await client.DidNotReceive().QueueCreate(name);
        }

        [Test]
        public async Task Delete()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();

            var init = new InitializeQueue(client, name);
            await init.Delete();

            await client.Received().QueueDelete(name);
        }
    }
}