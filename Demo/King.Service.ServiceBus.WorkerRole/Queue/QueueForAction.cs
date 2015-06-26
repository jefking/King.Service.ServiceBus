namespace King.Service.WorkerRole.Queue
{
    using System;
    using System.Diagnostics;
    using King.Service.ServiceBus;
    using King.Service.WorkerRole.Models;

    public class QueueForAction : RecurringTask
    {
        private readonly IBusQueueSender client;

        private readonly string action = null;

        public QueueForAction(IBusQueueSender client, string action)
            :base(30)
        {
            this.client = client;
            this.action = action;
        }

        public override void Run()
        {
            var model = new ExampleModel()
            {
                Identifier = Guid.NewGuid(),
                Action = action,
            };

            Trace.TraceInformation("Sending to queue for {0}: '{1}'", model.Action, model.Identifier);

            client.Send(model).Wait();
        }
    }
}