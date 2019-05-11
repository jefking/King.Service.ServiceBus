namespace King.Service.ServiceBus.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Demo.Models;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Example of Task class which adds an employee to a topic
    /// </summary>
    public class EmployeeQueuer : RecurringTask
    {
        private volatile int id = 0;
        private volatile bool isRad;
        private readonly IBusMessageSender queue = null;

        public EmployeeQueuer(IBusTopicClient client)
            : base(5)
        {
            this.queue = new BusMessageSender(client);
        }

        public override void Run()
        {
            var data = new EmployeeModel()
            {
                Id = Guid.NewGuid(),
                IsRad = isRad,
                Count = id,
            };
            
            Trace.TraceInformation("Queuing: '{0}' (ID: {1}:{2})", data.IsRad, data.Count, data.Id);
            this.queue.Send(data).Wait();
            
            id++;
            isRad = !isRad;
        }
    }
}