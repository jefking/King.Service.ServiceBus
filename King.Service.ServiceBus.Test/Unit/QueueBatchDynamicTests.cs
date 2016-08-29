namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class QueueBatchDynamicTests
    {
        const string ConnectionString = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new QueueBatchDynamic<object>(name, ConnectionString, processor);
        }

        [Test]
        public void IsDequeueBatchDynamic()
        {
            var name = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            Assert.IsNotNull(new QueueBatchDynamic<object>(name, ConnectionString, processor) as DequeueBatchDynamic<object>);
        }
    }
}