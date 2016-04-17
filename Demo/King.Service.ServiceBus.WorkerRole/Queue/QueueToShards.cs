namespace King.Service.WorkerRole.Queue
{
    using System;
    using System.Diagnostics;
    using King.Service.ServiceBus;
    using King.Service.WorkerRole.Models;

    public class QueueToShards : RecurringTask
    {
        private readonly IBusShardSender client;

        private readonly string action = null;

        public QueueToShards(IBusShardSender client)
            : base(2)
        {
            this.client = client;
        }

        public override void Run()
        {
            var model = new ExampleModel()
            {
                Identifier = Guid.NewGuid(),
                Action = action,
            };

            Trace.TraceInformation("Sending to queue shard: '{0}'", model.Identifier);

            client.Save(model).Wait();
        }
    }
}