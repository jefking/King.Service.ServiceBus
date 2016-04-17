namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using King.Service.ServiceBus.Timing;
    using NUnit.Framework;

    [TestFixture]
    public class TopicTimingTrackerTests
    {
        [Test]
        public void Constructor()
        {
            new TopicTimingTracker();
        }
    }
}