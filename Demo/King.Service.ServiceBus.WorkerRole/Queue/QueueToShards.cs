namespace King.Service.WorkerRole.Queue
{
    using Azure.Data;
    using King.Service.ServiceBus;
    using King.Service.WorkerRole.Models;
    using System;
    using System.Diagnostics;

    public class QueueToShards : RecurringTask
    {
        private readonly IQueueShardSender<IBusQueueSender> client;

        private readonly string action = null;

        public QueueToShards(IQueueShardSender<IBusQueueSender> client)
            :base(2)
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