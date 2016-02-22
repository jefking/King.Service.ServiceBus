namespace King.Service.WorkerRole.Queue
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole.Models;
    using System;
    using System.Diagnostics;

    public class EventHandler : ExampleProcessor, IBusEventHandler<ExampleModel>
    {
        public void OnError(string action, Exception ex)
        {
            Trace.TraceError(ex.ToString());
        }
    }
}