namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class InitializeRuleTaskTests
    {
        private string conn = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new InitializeRuleTask(conn, "fake", "none", "all", new SqlFilter("0=0"));
        }

        [Test]
        public void ConstructorTopicNull()
        {
            Assert.That(() => new InitializeRuleTask(conn, null, "sub", "all", new SqlFilter("0=0")), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorSubNull()
        {
            Assert.That(() => new InitializeRuleTask(conn, "topic", null, "all", new SqlFilter("0=0")), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorRuleNameNull()
        {
            Assert.That(() => new InitializeRuleTask(conn, "topic", "sub", null, new SqlFilter("0=0")), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorFilterNull()
        {
            Assert.That(() => new InitializeRuleTask(conn, "topic", "sub", "all", null), Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new InitializeRuleTask((IBusManagementClient)null, conn, "sub", "all", new SqlFilter("0=0")), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Create()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var name = "rule";
            var filter = new SqlFilter("0=0");
            var client = Substitute.For<IBusManagementClient>();

            var init = new InitializeRuleTask(client, topic, sub, name, filter);
            await init.RunAsync();

            await client.Received().RuleGet(topic, sub, Arg.Any<string>());
            await client.Received().RuleDelete(topic, sub, Arg.Any<string>());
            await client.Received().RuleCreate(topic, sub, name, filter);
        }

        [Test]
        public async Task CreateDontDelete()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var name = "rule";
            var filter = new SqlFilter("0=0");
            var client = Substitute.For<IBusManagementClient>();

            var init = new InitializeRuleTask(client, topic, sub, name, filter, false);
            await init.RunAsync();

            await client.DidNotReceive().RuleGet(topic, sub, Arg.Any<string>());
            await client.DidNotReceive().RuleDelete(topic, sub, Arg.Any<string>());
            await client.Received().RuleCreate(topic, sub, name, filter);
        }

        [Test]
        public async Task CreateGetThrows()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var name = "rule";
            var filter = new SqlFilter("0=0");
            var client = Substitute.For<IBusManagementClient>();
            client.When(c => c.RuleGet(topic, sub, Arg.Any<string>())).Do(x => { throw new Exception(); });

            var init = new InitializeRuleTask(client, topic, sub, name, filter);
            await init.RunAsync();

            await client.DidNotReceive().RuleDelete(topic, sub, Arg.Any<string>());
            await client.Received().RuleCreate(topic, sub, name, filter);
        }

        [Test]
        public async Task CreateDeleteThrows()
        {
            var random = new Random();
            var topic = string.Format("a{0}b", random.Next());
            var sub = string.Format("a{0}b", random.Next());
            var name = "rule";
            var filter = new SqlFilter("0=0");
            var client = Substitute.For<IBusManagementClient>();
            client.When(c => c.RuleDelete(topic, sub, Arg.Any<string>())).Do(x => { throw new Exception(); });

            var init = new InitializeRuleTask(client, topic, sub, name, filter);
            await init.RunAsync();

            await client.Received().RuleGet(topic, sub, Arg.Any<string>());
            await client.Received().RuleCreate(topic, sub, name, filter);
        }
    }
}