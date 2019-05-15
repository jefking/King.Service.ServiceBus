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
        private readonly bool topEarners;
        public EmployeeProcessor(bool topEarners)
        {
            this.topEarners = topEarners;
        }

        public Task<bool> Process(EmployeeModel data)
        {
            Trace.TraceInformation("Employee action {3}: '{0}' (ID: {1}:{2})", data.Salary, data.Count, data.Id, topEarners ? "top" : "base");

            return Task.FromResult<bool>(true);
        }

        public Task OnError(string action, Exception ex)
        {
            Trace.TraceError("Errored on {0}: {1}", action, ex.Message);

            return Task.FromResult<bool>(true);
        }
    }
}