namespace King.Service.ServiceBus.Test.Unit
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TransientErrorArgsTests
    {
        [Test]
        public void Constructor()
        {
            //new TransientErrorArgs();

            Assert.Fail();
        }

        [Test]
        public void IsEventArgs()
        {
            //Assert.IsNotNull(new TransientErrorArgs() as EventArgs);

            Assert.Fail();
        }

        [Test]
        public void Exception()
        {
            //var expected = new MessagingException(Guid.NewGuid().ToString());
            //var args = new TransientErrorArgs
            //{
            //    Exception = expected,
            //};

            //Assert.AreEqual(expected, args.Exception);

            Assert.Fail();
        }
    }
}