namespace King.Service.ServiceBus.Unit.Tests.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NSubstitute;
    using NUnit.Framework;
    using King.Service.ServiceBus.Wrappers;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Timing;

    [TestFixture]
    public class BufferedMessageEventHandlerTests
    {
        [Test]
        public void Constructor()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            var sleep = Substitute.For<ISleep>();
            new BufferedMessageEventHandler<object>(handler, sleep);
        }

        [Test]
        public void IsIBusEventHandlerBufferedMessage()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            var sleep = Substitute.For<ISleep>();
            Assert.IsNotNull(new BufferedMessageEventHandler<object>(handler, sleep) as IBusEventHandler<BufferedMessage>);
        }
    }
}