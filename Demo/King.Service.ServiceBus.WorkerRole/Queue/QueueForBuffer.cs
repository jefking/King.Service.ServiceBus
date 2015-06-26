namespace King.Service.WorkerRole.Queue
{
    using System;
    using System.Diagnostics;
    using King.Service.ServiceBus;
    using King.Service.WorkerRole.Models;

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
            var model = new BufferedModel
            {
                Identifier = Guid.NewGuid(),
                Action = "Buffered",
                ShouldProcessAt = DateTime.UtcNow.AddSeconds(15),
            };

            //Trace.TraceInformation("Sending to queue for {0}: '{1}'", model.Action, model.Identifier);

            client.SendBuffered(model, model.ShouldProcessAt).Wait();
        }
    }
}