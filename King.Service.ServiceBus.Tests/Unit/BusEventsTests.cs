namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusEventsTests
    {
        [Test]
        public void ConstructorMockable()
        {
            var queue = Substitute.For<IBusMessageReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BusEvents<object>(queue, handler);
        }

        [Test]
        public void ConstructorConcurrentOne()
        {
            var queue = Substitute.For<IBusMessageReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            var be = new BusEvents<object>(queue, handler, 1);
            Assert.AreEqual(BusEvents<object>.DefaultConcurrentCalls, be.ConcurrentCalls);
        }

        [Test]
        public void ConstructorNameNull()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            Assert.That(() => new BusEvents<object>(null, handler), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorConnectionStringNull()
        {
            var queue = Substitute.For<IBusMessageReciever>();
            Assert.That(() => new BusEvents<object>(queue, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Run()
        {
            var args = new ExceptionReceivedEventArgs(new Exception(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusMessageReciever>();
            queue.RegisterForEvents(Arg.Any<Func<IMessageSession, Message, CancellationToken, Task>>(), Arg.Any<SessionHandlerOptions>());
            var handler = Substitute.For<IBusEventHandler<object>>();

            var events = new BusEvents<object>(queue, handler);
            events.Run();

            queue.Received().RegisterForEvents(Arg.Any<Func<IMessageSession, Message, CancellationToken, Task>>(), Arg.Any<SessionHandlerOptions>());
        }

        [Test]
        public async Task OnMessageArrived()
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                var data = Guid.NewGuid();
                formatter.Serialize(stream, data);
                var msg = new Message(stream.ToArray());
                var queue = Substitute.For<IBusMessageReciever>();
                var handler = Substitute.For<IBusEventHandler<Guid>>();
                handler.Process(data).Returns(Task.FromResult(true));
                var session = Substitute.For<IMessageSession>();
                var ct = new CancellationToken();

                var events = new BusEvents<Guid>(queue, handler);
                await events.OnMessageArrived(session, msg, ct);

                await handler.Received().Process(data);
            }
        }

        [Test]
        public void MessageArrivedNotSuccessful()
        {
            var data = Guid.NewGuid().ToString();
            var queue = Substitute.For<IBusMessageReciever>();
            var handler = Substitute.For<IBusEventHandler<string>>();
            handler.Process(data).Returns(Task.FromResult(false));

            var events = new BusEvents<string>(queue, handler);

            Assert.That(() => events.Process(data), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void OnExceptionReceived()
        {
            var args = new ExceptionReceivedEventArgs(new Exception(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusMessageReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.OnError(args.ExceptionReceivedContext.Action, args.Exception);

            var events = new BusEvents<object>(queue, handler);
            events.OnExceptionReceived(args);

            handler.Received().OnError(args.ExceptionReceivedContext.Action, args.Exception);
        }

        [Test]
        public void OnExceptionReceivedExceptionNull()
        {
            var args = new ExceptionReceivedEventArgs(null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var queue = Substitute.For<IBusMessageReciever>();
            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.OnError(Arg.Any<string>(), Arg.Any<Exception>());

            var events = new BusEvents<object>(queue, handler);
            events.OnExceptionReceived(args);

            handler.Received(0).OnError(Arg.Any<string>(), Arg.Any<Exception>());
        }
    }
}