namespace King.Service.WorkerRole.Queue
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Queue;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class EventHandler : IBusEventHandler<ExampleModel>
    {
        public void OnError(string action, Exception ex)
        {
            Trace.TraceError(ex.ToString());
        }

        public Task<bool> Process(ExampleModel data)
        {
            Trace.TraceInformation("{0} {1}", data.Identifier, data.Action);

            return Task.FromResult(true);
        }
    }
}