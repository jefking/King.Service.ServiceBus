namespace King.Service.WorkerRole.Queue
{
    using System;
    using System.Diagnostics;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole.Models;

    public class EventHandler : ExampleProcessor, IBusEventHandler<ExampleModel>
    {
        public void OnError(string action, Exception ex)
        {
            Trace.TraceError(ex.ToString());
        }
    }
}