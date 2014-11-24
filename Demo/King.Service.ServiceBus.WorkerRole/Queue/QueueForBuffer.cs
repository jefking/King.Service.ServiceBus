namespace King.Service.WorkerRole.Queue
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Queue;
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
            var b = new BufferedMessage<ExampleModel>()
            {
                Data = new ExampleModel()
                {
                    Identifier = Guid.NewGuid(),
                    Action = "Buffered",
                },
                ReleaseAt = DateTime.UtcNow.AddSeconds(30),
            };

            Trace.TraceInformation("Sending to queue for {0}: '{1}'", b.Data.Action, b.Data.Identifier);

            client.Send(b).Wait();
        }
    }
}