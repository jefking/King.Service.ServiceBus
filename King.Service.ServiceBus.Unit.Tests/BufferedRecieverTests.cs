namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Timing;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BufferedRecieverTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BufferedReciever<object>(queue, handler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorSleepNull()
        {
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BufferedReciever<object>(queue, handler, null, 10);
        }

        [Test]
        public void IsBusEvents()
        {
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            Assert.IsNotNull(new BufferedReciever<object>(queue, handler) as BusEvents<object>);
        }

        [Test]
        public void DefaultConcurrentCalls()
        {
            Assert.AreEqual(50, BufferedReciever<object>.DefaultConcurrentCalls);
        }

        [Test]
        public async Task OnMessageArrived()
        {
            var g = Guid.NewGuid();
            var d = new BufferedMessage
            {
                ReleaseAt = DateTime.UtcNow,
                Data = JsonConvert.SerializeObject(g),
            };
            var msg = new BrokeredMessage(d);

            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<Guid>>();
            handler.Process(g).Returns(Task.FromResult(true));
            var sleep = Substitute.For<ISleep>();
            sleep.Until(d.ReleaseAt);

            var br = new BufferedReciever<Guid>(queue, handler, sleep);
            await br.OnMessageArrived(msg);

            handler.Received().Process(g);
            sleep.Received().Until(d.ReleaseAt);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task OnMessageArrivedNoSuccess()
        {
            var g = Guid.NewGuid();
            var d = new BufferedMessage
            {
                ReleaseAt = DateTime.UtcNow,
                Data = JsonConvert.SerializeObject(Guid.NewGuid()),
            };
            var msg = new BrokeredMessage(d);

            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<Guid>>();
            handler.Process(g).Returns(Task.FromResult(false));
            var sleep = Substitute.For<ISleep>();
            sleep.Until(d.ReleaseAt);

            var br = new BufferedReciever<Guid>(queue, handler, sleep);
            await br.OnMessageArrived(msg);

        }
    }
}