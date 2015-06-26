namespace King.Service.WorkerRole.Queue
{
    using System;
    using System.Diagnostics;
    using King.Service.ServiceBus;
    using King.Service.WorkerRole.Models;

    public class QueueForBuffer : RecurringTask
    {
        private readonly IBusQueueSender client;

        public QueueForBuffer(IBusQueueSender client)
        {
            this.client = client;
        }

        public override void Run()
        {
            var model = new ExampleModel
            {
                Identifier = Guid.NewGuid(),
                Action = "Buffered",
            };

            Trace.TraceInformation("Sending to queue for {0}: '{1}'", model.Action, model.Identifier);

            client.SendBuffered(model, DateTime.UtcNow.AddSeconds(20)).Wait();
        }
    }
}