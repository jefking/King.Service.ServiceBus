namespace King.Service.ServiceBus.Demo.Tasks
{
    using Newtonsoft.Json;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Core;
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
        private readonly IBusMessageSender queue = null;

        public EmployeeQueuer(IBusTopicClient client)
            : base(5)
        {
            this.queue = new BusMessageSender(client);
        }

        public override void Run()
        {
            var rand = new Random();
            var obj = new EmployeeModel()
            {
                Id = Guid.NewGuid(),
                Salary = rand.Next(1000),
                Count = id,
            };
            
            var j = JsonConvert.SerializeObject(obj);
            var data = System.Text.Encoding.Default.GetBytes(j);

            var msg = new Message(data)
            {
                ContentType = obj.GetType().ToString(),
                UserProperties =
                {
                    {"encoding", (byte)Encoding.Json},
                    {"salary", obj.Salary}
                }
            };
            
            Trace.TraceInformation("Queuing Employee ID: {0}:{1}", obj.Count, obj.Id);
            this.queue.Send(msg).Wait();
            
            id++;
        }
    }
}