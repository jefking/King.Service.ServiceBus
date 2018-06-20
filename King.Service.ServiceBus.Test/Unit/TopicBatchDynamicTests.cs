namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TopicBatchDynamicTests
    {
        const string ConnectionString = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new TopicBatchDynamic<object>(name, ConnectionString, subscription, processor);
        }

        [Test]
        public void IsDequeueBatchDynamic()
        {
            var name = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            Assert.IsNotNull(new TopicBatchDynamic<object>(name, ConnectionString, subscription, processor) as DequeueBatchDynamic<object>);
        }
    }
}