namespace King.Service.WorkerRole.Queue
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Queue;
    using King.Service.ServiceBus.Unit.Tests.Models;
    using System;
    using System.Diagnostics;

    public class QueueForBuffer : RecurringTask
    {
        #region Members
        private readonly IBusQueueSender client;
        #endregion

        #region Constructors
        public QueueForBuffer(IBusQueueSender client)
        {
            this.client = client;
        }
        #endregion

        public override void Run()
        {
            var model = new ExampleModel()
            {
                Identifier = Guid.NewGuid(),
                Action = "Buffered",
            };

            var b = new BufferedMessage()
            {
                Data = model,
                ReleaseAt = DateTime.UtcNow.AddSeconds(30),
            };

            Trace.TraceInformation("Sending to queue for {0}: '{1}'", model.Action, model.Identifier);

            client.Send(b).Wait();
        }
    }
}