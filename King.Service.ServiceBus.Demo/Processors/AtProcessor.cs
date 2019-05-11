namespace King.Service.ServiceBus.Demo.Processors
{
    using King.Service.ServiceBus.Demo.Models;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// At Processor, occurs post dequeue action
    /// </summary>
    public class AtProcessor : IBusEventHandler<AtModel>
    {
        public Task<bool> Process(AtModel data)
        {
            Trace.TraceInformation("Processing: '{0}' (ID: {1})", data.At, data.Id);

            return Task.FromResult<bool>(true);
        }

        public Task OnError(string action, Exception ex)
        {
            Trace.TraceError("Errored on {0}: {1}", action, ex.Message);

            return Task.FromResult<bool>(true);
        }
    }
}