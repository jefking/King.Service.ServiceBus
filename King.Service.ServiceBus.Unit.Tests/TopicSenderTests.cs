namespace King.Service.ServiceBus.Unit.Tests
{
    using NUnit.Framework;

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