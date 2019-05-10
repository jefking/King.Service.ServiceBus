namespace King.Service.ServiceBus.Test.Unit.Wrappers
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus.Management;
    using Microsoft.Azure.ServiceBus;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusManagementClientTests
    {
        private static readonly string connection = King.Service.ServiceBus.Test.Unit.Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new BusManagementClient(connection);
        }

        [Test]
        public void ConstructorNullClient()
        {
            Assert.That(() => new BusManagementClient((ManagementClient)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Client()
        {
            var client = new BusManagementClient(connection);
            Assert.IsNotNull(client.Client);
        }
    }
}