namespace King.Service.WorkerRole.Topic
{
    using System;
    using System.Diagnostics;
    using King.Service.ServiceBus;
    using King.Service.WorkerRole.Models;

    public class TopicShipper : RecurringTask
    {
        #region Members
        private readonly ITopicSender client;

        private readonly string action = null;
        #endregion

        #region Constructors
        public TopicShipper(ITopicSender client)
            :base(30)
        {
            this.client = client;
            this.action = "topic";
        }
        #endregion

        public override void Run()
        {
            var model = new ExampleModel()
            {
                Identifier = Guid.NewGuid(),
                Action = action,
            };

            Trace.TraceInformation("Sending to topic for {0}: '{1}'", model.Action, model.Identifier);

            client.Send(model).Wait();
        }
    }
}