﻿namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using global::Azure.Data.Wrappers;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicSubscriptionTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new BusTopicSubscription(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString());
        }

        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new BusTopicSubscription(null, connection, Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorConnectionNull()
        {
            Assert.That(() => new BusTopicSubscription(Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorSubscriptionNameNull()
        {
            Assert.That(() => new BusTopicSubscription(Guid.NewGuid().ToString(), connection, null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new BusTopicSubscription(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString()) as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var s = new BusTopicSubscription(Guid.NewGuid().ToString(), connection, name);

            Assert.AreEqual(name, s.Name);
        }

        [Test]
        public void TopicName()
        {
            var name = Guid.NewGuid().ToString();
            var s = new BusTopicSubscription(name, connection, Guid.NewGuid().ToString());

            Assert.AreEqual(name, s.TopicName);
        }

        [Test]
        public void Filter()
        {
            var filter = Guid.NewGuid().ToString();
            var s = new BusTopicSubscription(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString(), filter);

            Assert.AreEqual(filter, s.Filter);
        }

        [Test]
        public void FilterNone()
        {
            var s = new BusTopicSubscription(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString());

            Assert.IsNull(s.Filter);
        }

        [Test]
        public void FilterWhiteSpace()
        {
            var s = new BusTopicSubscription(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString(), "   ");

            Assert.IsNull(s.Filter);
        }
    }
}