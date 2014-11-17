namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusEventsTests
    {
        [Test]
        public void ConstructorMockable()
        {
            var queue = Substitute.For<IBusQueue>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BusEvents<object>(queue, handler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNameNull()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BusEvents<object>(null, handler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorConnectionStringNull()
        {
            var queue = Substitute.For<IBusQueue>();
            new BusEvents<object>(queue, null);
        }

        [Test]
        public void OnExceptionReceived()
        {
            var args = new ExceptionReceivedEventArgs(new Exception(), Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusQueue>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.OnError(args.Action, args.Exception);

            var events = new BusEvents<object>(queue, handler);
            events.OnExceptionReceived(new object(), args);

            handler.Received().OnError(args.Action, args.Exception);
        }

        [Test]
        public void OnExceptionReceivedSenderNull()
        {
            var args = new ExceptionReceivedEventArgs(new Exception(), Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusQueue>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.OnError(args.Action, args.Exception);

            var events = new BusEvents<object>(queue, handler);
            events.OnExceptionReceived(null, args);

            handler.Received().OnError(args.Action, args.Exception);
        }


        [Test]
        public void OnExceptionReceivedExceptionNull()
        {
            var args = new ExceptionReceivedEventArgs(null, Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusQueue>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.OnError(Arg.Any<string>(), Arg.Any<Exception>());

            var events = new BusEvents<object>(queue, handler);
            events.OnExceptionReceived(new object(), args);

            handler.Received(0).OnError(Arg.Any<string>(), Arg.Any<Exception>());
        }
    }
}