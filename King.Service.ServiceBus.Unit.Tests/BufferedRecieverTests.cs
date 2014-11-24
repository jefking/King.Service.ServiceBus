namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus.Unit.Tests.Models;
    using King.Service.ServiceBus.Unit.Tests.Timing;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
        public async Task OnMessageArrived()
        {
            var d = new BufferedMessage
            {
                ReleaseAt = DateTime.UtcNow,
                Data = Guid.NewGuid(),
            };
            var msg = new BrokeredMessage(d);

            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.Process(d.Data).Returns(Task.FromResult(true));
            var sleep = Substitute.For<ISleep>();
            sleep.Until(d.ReleaseAt);

            var br = new BufferedReciever<object>(queue, handler, sleep);
            await br.OnMessageArrived(msg);

            handler.Received().Process(d.Data);
            sleep.Received().Until(d.ReleaseAt);
        }
    }
}