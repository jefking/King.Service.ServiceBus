namespace King.Service.ServiceBus.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Demo.Models;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Example of Task class which adds a company to a queue
    /// </summary>
    public class AtQueuer : RecurringTask
    {
        private int id = 0;
        private readonly IBusMessageSender queue = null;

        public AtQueuer(IBusQueueClient client)
            : base(5)
        {
            this.queue = new BusMessageSender(client);
        }

        public override void Run()
        {
            var data = new AtModel()
            {
                Id = Guid.NewGuid(),
                At = DateTime.UtcNow.AddSeconds(10),
            };
            
            Trace.TraceInformation("Dequeue At: '{0}' (ID: {1})", data.At, data.Id);
            this.queue.SendBuffered(data, data.At).Wait();
            
            id++;
        }
    }
}