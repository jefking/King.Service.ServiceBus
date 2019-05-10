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
    public class CompanyQueuer : RecurringTask
    {
        private int id = 0;
        private readonly IBusMessageSender queue = null;

        public CompanyQueuer(IBusQueueClient client)
            :base(5)
        {
            this.queue = new BusMessageSender(client);
        }

        public override void Run()
        {
            var data = new CompanyModel()
            {
                Id = Guid.NewGuid(),
                Name = string.Format("company-{0}", id),
                Count = id,
            };
            
            Trace.TraceInformation("Queuing: '{0}' (ID: {1}:{2})", data.Name, data.Count, data.Id);
            this.queue.Send(data).Wait();
            
            id++;
        }
    }
}