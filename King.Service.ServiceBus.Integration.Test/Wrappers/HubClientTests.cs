﻿namespace King.Service.ServiceBus.Integration.Test.Wrappers
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Data;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;

    [TestFixture]
    public class HubClientTests
    {
        private string connection = ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];

        string name;

        [SetUp]
        public void Setup()
        {
            var random = new Random();
            this.name = string.Format("a{0}b", random.Next());

            var init = new InitializeStorageTask(new BusHub(name, connection));
            init.Run();
        }

        [Test]
        public async Task Send()
        {
            var msg = new EventData(Guid.NewGuid().ToByteArray());
            var bq = new HubClient(EventHubClient.CreateFromConnectionString(connection, this.name));
            await bq.Send(msg);
        }

        [Test]
        public async Task SendBatch()
        {
            var msgs = new EventData[] { new EventData(Guid.NewGuid().ToByteArray()), new EventData(Guid.NewGuid().ToByteArray()), new EventData(Guid.NewGuid().ToByteArray()), new EventData(Guid.NewGuid().ToByteArray()) };

            var bq = new HubClient(EventHubClient.CreateFromConnectionString(connection, this.name));
            await bq.Send(msgs);
        }
    }
}