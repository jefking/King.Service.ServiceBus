namespace King.Service.ServiceBus.Demo.Processors
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Demo.Models;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Employee Processor, occurs post dequeue action
    /// </summary>
    public class EmployeeProcessor : IBusEventHandler<EmployeeModel>
    {
        public Task<bool> Process(EmployeeModel data)
        {
            Trace.TraceInformation("Processing: '{0}' (ID: {1}:{2})", data.IsRad, data.Count, data.Id);

            return Task.FromResult<bool>(true);
        }

        public Task OnError(string action, Exception ex)
        {
            Trace.TraceError("Errored on {0}: {1}", action, ex.Message);

            return Task.FromResult<bool>(true);
        }
    }
}