namespace King.Service.ServiceBus.Test.Unit.Models
{
    using System;
    using King.Service.ServiceBus.Models;
    using NUnit.Framework;

    [TestFixture]
    public class BufferedMessageTests
    {
        [Test]
        public void Constructor()
        {
            new BufferedMessage();
        }

        [Test]
        public void IsIBufferedMessage()
        {
            Assert.IsNotNull(new BufferedMessage() as IBufferedMessage);
        }

        [Test]
        public void Data()
        {
            var expected = Guid.NewGuid().ToString();
            var obj = new BufferedMessage()
            {
                Data = expected
            };

            Assert.AreEqual(expected, obj.Data);
        }

        [Test]
        public void ReleaseAt()
        {
            var expected = DateTime.UtcNow;
            var obj = new BufferedMessage()
            {
                ReleaseAt = expected
            };

            Assert.AreEqual(expected, obj.ReleaseAt);
        }
    }
}