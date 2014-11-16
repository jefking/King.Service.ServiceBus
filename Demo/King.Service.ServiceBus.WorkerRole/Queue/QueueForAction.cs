namespace King.Service.WorkerRole.Queue
{
    using King.Service.ServiceBus.Queue;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;

    public class QueueForAction : RecurringTask
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        private readonly QueueClient client;

        private readonly string action = null;
        #endregion

        #region Constructors
        public QueueForAction(QueueClient client, string action)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
            this.action = action;
        }
        #endregion

        public override void Run()
        {
            var model = new ExampleModel()
            {
                Identifier = Guid.NewGuid(),
                Action = action,
            };

            Trace.TraceInformation("Sending to Poll: '{0}/{1}'", model.Action, model.Identifier);

            client.Send(new BrokeredMessage(model));
        }
    }
}