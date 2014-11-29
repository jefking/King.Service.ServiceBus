namespace King.Service.ServiceBus.Unit.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class TopicSenderTests
    {
        [Test]
        public void Constructor()
        {
            new TopicSender();
        }

        [Test]
        public void IsITopicSender()
        {
            Assert.IsNotNull(new TopicSender() as ITopicSender);
        }
    }
}