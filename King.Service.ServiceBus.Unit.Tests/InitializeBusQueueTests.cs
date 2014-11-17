namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class InitializeBusQueueTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusQueue>();
            new InitializeBusQueue(queue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            new InitializeBusQueue(null);
        }

        [Test]
        public void IsInitializeTask()
        {
            var queue = Substitute.For<IBusQueue>();
            Assert.IsNotNull(new InitializeBusQueue(queue) as InitializeTask);
        }

        [Test]
        public async Task RunAsync()
        {
            var queue = Substitute.For<IBusQueue>();
            queue.CreateIfNotExists();

            var t = new InitializeBusQueue(queue);
            await t.RunAsync();

            queue.Received().CreateIfNotExists();
        }
    }
}