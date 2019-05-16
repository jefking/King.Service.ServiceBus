namespace King.Service.ServiceBus.Test.Integration.Wrappers
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusManagementClientTests
    {
        private static readonly string connection = King.Service.ServiceBus.Test.Integration.Configuration.ConnectionString;

        [Test]
        public async Task CreateQueue()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());

            var client = new BusManagementClient(connection);
            await client.QueueCreate(name);

            var exists = await client.QueueExists(name);
            Assert.IsTrue(exists);

            await client.QueueDelete(name);
        }

        [Test]
        public async Task DeleteQueue()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());

            var client = new BusManagementClient(connection);
            await client.QueueCreate(name);
            await client.QueueDelete(name);

            var exists = await client.QueueExists(name);
            Assert.IsFalse(exists);
        }
        
        [Test]
        public async Task QueueDoesntExist()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = new BusManagementClient(connection);

            var exists = await client.QueueExists(name);
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task CreateTopic()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());

            var client = new BusManagementClient(connection);
            await client.TopicCreate(name);

            var exists = await client.TopicExists(name);
            Assert.IsTrue(exists);

            //cleanup
            await client.TopicDelete(name);
        }

        [Test]
        public async Task DeleteTopic()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());

            var client = new BusManagementClient(connection);
            await client.TopicCreate(name);

            await client.TopicDelete(name);

            var exists = await client.TopicExists(name);
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task TopicDoesntExist()
        {
            var random = new Random();
            var name = string.Format("a{0}b", random.Next());
            var client = new BusManagementClient(connection);

            var exists = await client.TopicExists(name);
            Assert.IsFalse(exists);

            //cleanup
            await client.TopicDelete(name);
        }

        [Test]
        public async Task CreateSubscription()
        {
            var random = new Random();
            var topicName = string.Format("a{0}b", random.Next());
            var subName = "sub";

            var client = new BusManagementClient(connection);
            await client.TopicCreate(topicName);
            await client.SubscriptionCreate(topicName, subName);

            var exists = await client.SubscriptionExists(topicName, subName);
            Assert.IsTrue(exists);

            //cleanup
            await client.TopicDelete(topicName);
        }

        [Test]
        public async Task DeleteSubscription()
        {
            var random = new Random();
            var topicName = string.Format("a{0}b", random.Next());
            var subName = "sub";

            var client = new BusManagementClient(connection);
            await client.TopicCreate(topicName);
            await client.SubscriptionCreate(topicName, subName);

            await client.SubscriptionDelete(topicName, subName);

            var exists = await client.SubscriptionExists(topicName, subName);
            Assert.IsFalse(exists);

            //cleanup
            await client.TopicDelete(topicName);
        }
        
        [Test]
        public async Task SubscriptionDoesntExist()
        {
            var random = new Random();
            var topicName = string.Format("a{0}b", random.Next());
            var subName = "sub";
            var client = new BusManagementClient(connection);

            var exists = await client.SubscriptionExists(topicName, subName);
            Assert.IsFalse(exists);

            //cleanup
            await client.TopicDelete(topicName);
        }

        [Test]
        public async Task CreateRule()
        {
            var random = new Random();
            var topicName = string.Format("a{0}b", random.Next());
            var subName = "sub";
            var ruleName = "rule";

            var filter = new SqlFilter("0=0");

            var client = new BusManagementClient(connection);
            await client.TopicCreate(topicName);
            await client.SubscriptionCreate(topicName, subName);

            await client.RuleCreate(topicName, subName, ruleName, filter);

            var rule = await client.RuleGet(topicName, subName, ruleName);
            Assert.IsNotNull(rule);
            Assert.AreEqual(ruleName, rule.Name);

            //cleanup
            await client.TopicDelete(topicName);
        }

        [Test]
        public async Task DeleteRule()
        {
            var random = new Random();
            var topicName = string.Format("a{0}b", random.Next());
            var subName = "sub";
            var ruleName = "rule";

            var filter = new SqlFilter("0=0");

            var client = new BusManagementClient(connection);
            await client.TopicCreate(topicName);
            await client.SubscriptionCreate(topicName, subName);

            await client.RuleCreate(topicName, subName, ruleName, filter);

            await client.RuleDelete(topicName, subName, ruleName);

            try
            {
                await client.RuleGet(topicName, subName, ruleName);
                Assert.Fail();
            }
            catch
            {
            }

            //cleanup
            await client.TopicDelete(topicName);
        }
    }
}