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
    }
}