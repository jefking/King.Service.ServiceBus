namespace King.Service.ServiceBus.Demo.Processors
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Demo.Models;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Company Processor, occurs post dequeue action
    /// </summary>
    public class CompanyProcessor : IBusEventHandler<CompanyModel>
    {
        public Task<bool> Process(CompanyModel data)
        {
            Trace.TraceInformation("Processing: '{0}' ({1}:{2})", data.Name, data.Count, data.Id);

            return Task.FromResult<bool>(true);
        }

        public Task OnError(string action, Exception ex)
        {
            Trace.TraceError("Errored on {0}: {1}", action, ex.Message);

            return Task.FromResult<bool>(true);
        }
    }
}