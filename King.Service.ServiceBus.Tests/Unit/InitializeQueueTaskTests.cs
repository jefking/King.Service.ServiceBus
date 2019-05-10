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
    public class InitializeQueueTaskTests
    {
        private string conn = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new InitializeQueueTask("fake", conn);
        }
        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new InitializeQueueTask((string)null, conn), Throws.TypeOf<ArgumentNullException>());
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
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.QueueExists(name).Returns(false);

            var init = new InitializeQueueTask(client, name);
            init.Run();

            client.Received().QueueExists(name);
            client.Received().QueueCreate(name);
        }

        [Test]
        public void CreateExists()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = Substitute.For<IBusManagementClient>();
            client.QueueExists(name).Returns(true);

            var init = new InitializeQueueTask(client, name);
            init.Run();

            client.Received().QueueExists(name);
            client.DidNotReceive().QueueCreate(name);
        }
    }
}