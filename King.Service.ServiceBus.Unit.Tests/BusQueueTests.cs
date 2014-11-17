namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueTests
    {
        [Test]
        public void Constructor()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            new BusQueue(Guid.NewGuid().ToString(), fake);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            new BusQueue(null, fake);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new BusQueue(Guid.NewGuid().ToString(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveObjectNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var queue = new BusQueue(Guid.NewGuid().ToString(), fake);
            await queue.Save((object)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveBrokeredMessageNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var queue = new BusQueue(Guid.NewGuid().ToString(), fake);
            await queue.Save((BrokeredMessage)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterForEventsCallbackNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var queue = new BusQueue(Guid.NewGuid().ToString(), fake);
            queue.RegisterForEvents(null, new OnMessageOptions());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterForEventsOptionsNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var queue = new BusQueue(Guid.NewGuid().ToString(), fake);
            queue.RegisterForEvents(this.OnMessageArrived, null);
        }

        private async Task OnMessageArrived(BrokeredMessage message)
        {
            await new TaskFactory().StartNew(() => { });
        }
    }
}