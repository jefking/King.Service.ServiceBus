namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusEventsTests
    {
        [Test]
        public void ConstructorMockable()
        {
            var queue = Substitute.For<IBusQueueReciever>();
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
            var queue = Substitute.For<IBusQueueReciever>();
            new BusEvents<object>(queue, null);
        }

        [Test]
        public void Run()
        {
            var args = new ExceptionReceivedEventArgs(new Exception(), Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusQueueReciever>();
            queue.RegisterForEvents(Arg.Any<Func<BrokeredMessage, Task>>(), Arg.Any<OnMessageOptions>());
            var handler = Substitute.For<IBusEventHandler<object>>();

            var events = new BusEvents<object>(queue, handler);
            events.Run();

            queue.Received().RegisterForEvents(Arg.Any<Func<BrokeredMessage, Task>>(), Arg.Any<OnMessageOptions>());
        }

        [Test]
        public async Task OnMessageArrived()
        {
            var data = Guid.NewGuid().ToString();
            var msg = new BrokeredMessage(data)
            {
                ContentType = data.GetType().ToString(),
            };
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<string>>();
            handler.Process(data).Returns(Task.FromResult(true));

            var events = new BusEvents<string>(queue, handler);
            await events.OnMessageArrived(msg);

            handler.Received().Process(data);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task OnMessageArrivedNotProcessed()
        {
            var data = Guid.NewGuid().ToString();
            var msg = new BrokeredMessage(data)
            {
                ContentType = data.GetType().ToString(),
            };
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<string>>();
            handler.Process(data).Returns(Task.FromResult(false));

            var events = new BusEvents<string>(queue, handler);
            await events.OnMessageArrived(msg);

            handler.Received().Process(data);
        }

        [Test]
        public void OnExceptionReceived()
        {
            var args = new ExceptionReceivedEventArgs(new Exception(), Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusQueueReciever>();
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
            var queue = Substitute.For<IBusQueueReciever>();
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
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.OnError(Arg.Any<string>(), Arg.Any<Exception>());

            var events = new BusEvents<object>(queue, handler);
            events.OnExceptionReceived(new object(), args);

            handler.Received(0).OnError(Arg.Any<string>(), Arg.Any<Exception>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBodyMessageNull()
        {
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            
            var be = new BusEvents<object>(queue, handler);
            be.GetBody(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBodyContentTypeNull()
        {
            var queue = Substitute.For<IBusQueueReciever>();
            var handler = Substitute.For<IBusEventHandler<Guid>>();

            var msg = new BrokeredMessage(Guid.NewGuid());

            var be = new BusEvents<Guid>(queue, handler);
            be.GetBody(msg);
        }
    }
}