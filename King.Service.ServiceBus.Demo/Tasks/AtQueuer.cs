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
                At = DateTime.UtcNow.AddSeconds(5),
            };
            
            Trace.TraceInformation("Queuing: '{0}' (ID: {1}:{2})", data.Id);
            this.queue.Send(data, data.At).Wait();
            
            id++;
        }
    }
}