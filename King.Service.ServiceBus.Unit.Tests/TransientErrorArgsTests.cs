namespace King.Service.ServiceBus.Unit.Tests
{
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TransientErrorArgsTests
    {
        [Test]
        public void Constructor()
        {
            new TransientErrorArgs();
        }

        [Test]
        public void IsEventArgs()
        {
            Assert.IsNotNull(new TransientErrorArgs() as EventArgs);
        }

        [Test]
        public void Exception()
        {
            var expected = new MessagingException(Guid.NewGuid().ToString());
            var args = new TransientErrorArgs
            {
                Exception = expected,
            };

            Assert.AreEqual(expected, args.Exception);
        }
    }
}