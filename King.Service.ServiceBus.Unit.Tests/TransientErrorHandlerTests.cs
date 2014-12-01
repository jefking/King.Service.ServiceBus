namespace King.Service.ServiceBus.Unit.Tests
{
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TransientErrorHandlerTests
    {
        [Test]
        public void Constructor()
        {
            new TransientErrorHandler();
        }

        [Test]
        public void IsITransientErrorHandler()
        {
            Assert.IsNotNull(new TransientErrorHandler() as ITransientErrorHandler);
        }

        [Test]
        public void HandleTransientError()
        {
            var ex = new MessagingException(Guid.NewGuid().ToString());

            var h = new TransientErrorHandler();
            h.TransientErrorOccured += h_TransientErrorOccured;
            h.HandleTransientError(ex);

            Assert.AreEqual(ex, this.exception);
        }

        void h_TransientErrorOccured(object sender, TransientErrorArgs e)
        {
            this.exception = e.Exception;
        }

        MessagingException exception = null;
    }
}